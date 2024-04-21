using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlNado;
using SyslogLogging;

namespace Mhora;

internal static class Application
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

	private static SQLiteDatabase _worldDb;
	public static  SQLiteDatabase WorldDb => _worldDb;

	public static async Task InitDb()
	{
		_worldDb = new SQLiteDatabase("world.db");
	}

	private const string LogFile = "debug.txt";

	/// <summary>
	///     The main entry point for the application.
	/// </summary>
	[STAThread]
	private static void Main(string[] args)
	{
		Running                                          =  true;
		AppDomain.CurrentDomain.UnhandledException       += UnhandledExceptionEventRaised;
		System.Windows.Forms.Application.ThreadException += ThreadExceptionRaised;
		System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
		System.Windows.Forms.Application.EnableVisualStyles();
		System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

		if (File.Exists(LogFile))
		{
			File.Delete(LogFile);
		}

		Log.Settings.LogFilename = Path.Combine(WorkingDir, LogFile);
		Log.Settings.FileLogging = FileLoggingMode.SingleLogFile;

		System.Windows.Forms.Application.Run(new MainForm());

		_worldDb.Dispose();

		AppDomain.CurrentDomain.UnhandledException       -= UnhandledExceptionEventRaised;
		System.Windows.Forms.Application.ThreadException -= ThreadExceptionRaised;
		Running                                          =  false;
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