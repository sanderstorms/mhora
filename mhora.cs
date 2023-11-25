using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace mhora
{
    internal static class mhora
    {
        private static bool _running;
        private static string _workingDir;

        public static bool Running
        {
            get
            {
                return (_running);
            }
        }

        public static Assembly ActiveAssembly
        {
            get
            {
                if (_running)
                {
                    return (Assembly.GetEntryAssembly());
                }
                return (Assembly.GetExecutingAssembly());
            }
        }

        public static string WorkingDir
        {
            get
            {
                if (_workingDir == null)
                {
                    _workingDir = Path.GetDirectoryName(ActiveAssembly.Location);
                }
                return (_workingDir);
            }
        }

        internal static Version Version
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MhoraContainer());

            AppDomain.CurrentDomain.UnhandledException -= UnhandledExceptionEventRaised;
            Application.ThreadException -= ThreadExceptionRaised;
            _running = false;
        }


        private static void ThreadExceptionRaised(object sender, ThreadExceptionEventArgs e)
        {
        }

        private static void UnhandledExceptionEventRaised(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
        }
    }
}