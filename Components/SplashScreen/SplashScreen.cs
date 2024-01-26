// SplashScreen.cs: Contributed by Peter Foreman
// A splash screen class

#region Copyright © 2002-2004 The Genghis Group

/*
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from the
 * use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not claim
 * that you wrote the original software. If you use this software in a product,
 * an acknowledgment in the product documentation is required, as shown here:
 *
 * Portions Copyright © 2002-2004 The Genghis Group (http://www.genghisgroup.com/).
 *
 * 2. No substantial portion of the source code of this library may be redistributed
 * without the express written permission of the copyright holders, where
 * "substantial" is defined as enough code to be recognizably from this library.
 */

#endregion

#region Features

/*
 * -Makes a form into a splash screen
 * -Designed with a minimal interface to increase ease of use
 * -Multithreaded to ensure message queue is pumped whilst main thread is busy
 */

#endregion

#region Limitations

/*
 *
 */

#endregion

#region History

/*
 *
 */

#endregion

using System;
using System.Threading;
using System.Windows.Forms;
using Mhora.Components.SplashScreen;
using Timer = System.Windows.Forms.Timer;

namespace Genghis.Windows.Forms;

/// <summary>
///     Splash Screen class
/// </summary>
public class SplashScreen
{
	private readonly Type               formType;
	private readonly SplashScreenStyles styles            = SplashScreenStyles.None;
	private          bool               openWindowAllowed = true;
	private          Thread             thread;
	private          SplashScreenWindow window;

	/// <summary>
	///     Create a splash screen based on a Form.
	///     form must be System.Windows.Forms.Form or derived from it.
	///     Typical usage: <c>SplashScreen ss = new SplashScreen( typeof( MyForm ) );</c>.
	/// </summary>
	public SplashScreen(Type formType)
	{
		this.formType = formType;
		Initialize();
	}

	/// <summary>
	///     Create a splash screen based on a Form, with non standard styles.
	///     form must be System.Windows.Forms.Form or derived from it.
	///     Typical usage: <c>SplashScreen ss = new SplashScreen( typeof( MyForm ), SplashScreenStyles.TopMost ); );</c>.
	/// </summary>
	public SplashScreen(Type formType, SplashScreenStyles styles)
	{
		this.formType = formType;
		this.styles   = styles;
		Initialize();
	}

	private void Initialize()
	{
		if (!formType.IsSubclassOf(typeof(Form)))
		{
			throw new ArgumentException("The type passed in must be a Form, or subclass.", "formType");
		}

		CreateSplashScreenThread();
	}

	/// <summary>
	///     Closes the splash screen immediately.
	///     Activates the form specified by main (may be null).
	/// </summary>
	// Thread safe
	public void Close(Form main)
	{
		Close(main, 0);
	}

	/// <summary>
	///     Closes the splash screen after a specific time period (specified in milliseconds).
	///     Activates the form specified by main (may be null).
	/// </summary>
	// Thread safe
	public void Close(Form main, int milliseconds)
	{
		lock (this)
		{
			// It could (in theory) be possible to call Close() before the window has been created
			// on the thread - this flag prevents that happening
			openWindowAllowed = false;
			window?.Close(main, milliseconds);
		}
	}

	// Only call from ctor
	private void CreateSplashScreenThread()
	{
		thread                = new Thread(ThreadFunction);
		thread.Name           = "Splash Screen";
		thread.ApartmentState = ApartmentState.STA;

		thread.Start();
		Thread.Sleep(0);
	}

	// Only call when creating new thread in CreateSplashScreenThread
	private void ThreadFunction()
	{
		lock (this)
		{
			if (openWindowAllowed)
			{
				window = new SplashScreenWindow(formType, styles);
			}
		}

		window?.EnterMessagePump();
	}

	/// <summary>
	///     Internal class whose lifetime (between ctor and Close) determines how long the
	///     splash screen remains visible
	/// </summary>
	private class SplashScreenWindow
	{
		private Form form;
		private Form main;

		public SplashScreenWindow(Type formType, SplashScreenStyles styles)
		{
			CreateForm(formType);

			form.ShowInTaskbar = false;
			form.TopLevel      = true;

			if ((styles & SplashScreenStyles.TopMost) != 0)
			{
				form.TopMost = true; // Yuk!  Do you really have to
			}

			// If bit not set then set border
			if ((styles & SplashScreenStyles.DontSetBorderStyle) == 0)
			{
				form.FormBorderStyle = FormBorderStyle.FixedSingle;
			}

			// If bit not set then SetStyles
			if ((styles & SplashScreenStyles.DontSetFormStyles) == 0)
			{
				SetStyles();
			}
		}

		public void EnterMessagePump()
		{
			Application.Run(form);
		}

		//Call from ctor only
		private void CreateForm(Type formType)
		{
			// Get default constructor
			var constructor = formType.GetConstructor(Type.EmptyTypes);
			form = constructor.Invoke(null) as Form;
		}

		//Call from ctor only
		private void SetStyles()
		{
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MaximizeBox   = false;
			form.MinimizeBox   = false;
			form.Name          = string.Empty;
			form.Text          = string.Empty;
			form.ControlBox    = false;
		}

		//Thread safe
		public void Close(Form main, int milliseconds)
		{
			this.main = main;

			if (milliseconds <= 0)
			{
				CloseNow();
			}
			else
			{
				SetupCloseTimer(milliseconds);
			}
		}

		private void SetupCloseTimer(int milliseconds)
		{
			lock (this)
			{
				var timer = new Timer();
				timer.Interval =  milliseconds;
				timer.Tick     += ElapsedEventHandler;
				timer.Start();
			}
		}

		private void ElapsedEventHandler(object sender, EventArgs e)
		{
			var timer = sender as Timer;
			timer.Stop();
			CloseNow();
		}

		private void CloseNow()
		{
			lock (this)
			{
				main?.Activate();

				form?.Invoke(new CloseDelegate(form.Close));

				form = null;
			}
		}

		private delegate void CloseDelegate();
	} //class SplashScreenWindow
}     //class SplashScreen