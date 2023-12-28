using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mhora.Database;
using Mhora.Database.World;
using Newtonsoft.Json;
using SqlNado;
using SqlNado.Query;
using SyslogLogging;

namespace Mhora;

internal static class mhora
{
    private static string        _workingDir;
    private static LoggingModule _log;

    private static DataTable _countryCodes;

    internal static bool Running
    {
        get;
        private set;
    }

    internal static Assembly      ActiveAssembly => Running ? Assembly.GetEntryAssembly() : Assembly.GetExecutingAssembly();
    internal static string        WorkingDir     => _workingDir ??= Path.GetDirectoryName(ActiveAssembly.Location);
    internal static Version       Version        => Assembly.GetExecutingAssembly().GetName().Version;
    internal static LoggingModule Log            => _log ??= new LoggingModule();

    internal static string VersionString
    {
        get
        {
            var versionString = Version.ToString();

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

    public static TimeZones TimeZones
    {
        get;
        private set;
    }

    public static async Task InitDb()
    {
        _countryCodes = CountryCode.Instance.Table;
        var jsonPath = Path.Combine(WorkingDir, "DataBase", "TimeZones.json");

        // deserialize JSON directly from a file
        using (var file = File.OpenText(jsonPath))
        {
            var serializer = new JsonSerializer();
            try
            {
                TimeZones = (TimeZones) serializer.Deserialize(file, typeof(TimeZones));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        if (File.Exists("world.db"))
        {
            try
            {
                using var db     = new SQLiteDatabase("world.db");
                var       query  = Query.From<City>().Where(city => city.Name == "Amsterdam").SelectAll();
                var       cities = db.Load<City>(query.ToString()).ToArray();
                if (cities.Length > 0)
                {
                    foreach (var city in cities)
                    {
                        var country      = city.Country;
                        var timeZone     = TimeZones.FindId(country.Timezones[0].zoneName);
                        var translations = country.Translations;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
        Running                                    =  true;
        AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionEventRaised;
        Application.ThreadException                += ThreadExceptionRaised;
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        Log.Settings.LogFilename = Path.Combine(WorkingDir, "debug.txt");
        Log.Settings.FileLogging = FileLoggingMode.SingleLogFile;

        Application.Run(new MhoraContainer());
        AppDomain.CurrentDomain.UnhandledException -= UnhandledExceptionEventRaised;
        Application.ThreadException                -= ThreadExceptionRaised;
        Running                                    =  false;
    }


    private static void ThreadExceptionRaised(object sender, ThreadExceptionEventArgs e)
    {
        Log.Exception(e.Exception);
    }

    private static void UnhandledExceptionEventRaised(object sender, UnhandledExceptionEventArgs e)
    {
        var exception = (Exception) e.ExceptionObject;
        Log.Exception(exception);
    }
}