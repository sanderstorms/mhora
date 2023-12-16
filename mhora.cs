using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using mhora.Components;
using Mhora.Database;
using SqlNado;
using SyslogLogging;

namespace Mhora
{
    internal static class mhora
    {
        private static bool _running;
        private static string _workingDir;

        internal static bool Running => (_running);
        internal static Assembly ActiveAssembly => _running ? Assembly.GetEntryAssembly() : Assembly.GetExecutingAssembly();
        internal static string WorkingDir => (_workingDir ??= Path.GetDirectoryName(ActiveAssembly.Location));
        internal static Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        private static LoggingModule _log;
        internal static LoggingModule Log => (_log ??= new LoggingModule());

        internal static string VersionString
        {
            get
            {
                string versionString = Version.ToString();

                switch (Version.Build)
                {
                    case 0:
                    {
                        versionString += " (beta)";
                    }
                    break;

                    case 1:
                    {
                        versionString += " (release candidate)";
                    }
                    break;

                    case 2:
                    {
                    }
                    break;

                    default:
                    {
                        versionString += " (?)";
                    }
                    break;
                }

                return versionString;
            }
        }

        private static DataTable _geoNames;
        private static List<TimeZoneDb.Timezone> _timeZones;
        private static void InitDb()
        {
            _geoNames = GeoNamesDb.Instance.Table;
            _timeZones = TimeZoneDb.Instance.TimeZones;

            if (File.Exists("Countries.db") == false)
            {
                using (var db = new SQLiteDatabase("Countries.db"))
                {
                    db.SynchronizeSchema<TimeZoneDb.Timezone>();
                    foreach (var timeZone in _timeZones)
                    {
                        db.Save(timeZone);
                    }
                }
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            _running = true;
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionEventRaised;
            Application.ThreadException += ThreadExceptionRaised;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Log.Settings.LogFilename = Path.Combine(WorkingDir, "debug.txt");
            Log.Settings.FileLogging = FileLoggingMode.SingleLogFile;

            InitDb();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var birthDetails = new BirthDetailsDialog())
            {
                birthDetails.ShowDialog();
            }
            Application.Run(new MhoraContainer());
            AppDomain.CurrentDomain.UnhandledException -= UnhandledExceptionEventRaised;
            Application.ThreadException -= ThreadExceptionRaised;
            _running = false;
        }


        private static void ThreadExceptionRaised(object sender, ThreadExceptionEventArgs e)
        {
            Log.Exception(e.Exception);
        }

        private static void UnhandledExceptionEventRaised(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
            Log.Exception(exception);
        }
    }
}