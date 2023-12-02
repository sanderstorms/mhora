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

namespace Mhora.Components.SplashScreen
{
    [Flags]
    public enum SplashScreenStyles
    {
        None = 0,

        /// <summary>
        ///     Normally the splash is automatically set to be center of screen and without a title bar
        ///     overriding whatever styles normally apply.
        ///     Set this to avoid those styles being applied.
        /// </summary>
        DontSetFormStyles = 1,

        /// <summary>
        ///     Makes the created splash screen top most - not recommended as it can be irritating for users.
        ///     See Form.TopMost for more details.
        /// </summary>
        TopMost = 2,

        /// <summary>
        ///     Normally the splash is created with a FixedSingle border style.
        ///     Set this to avoid that style being applied.
        /// </summary>
        DontSetBorderStyle = 4
    }
}