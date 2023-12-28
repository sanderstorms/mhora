/******
This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
******/

// =====================================================================
//
// ShellLink - Using WSH to program shell links
//
// by Jim Hollenhorst, jwtk@ultrapico.com
// Copyright Ultrapico, April 2003
// http://www.ultrapico.com
//
// =====================================================================

using System;
using System.IO;
using System.Windows.Forms;
using IWshRuntimeLibrary;

namespace Mhora.Util;

/// <summary>
///     Summary description for Link.
/// </summary>
public class Link
{
    /// <summary>
    ///     Check to see if a shortcut exists in a given directory with a specified file name
    /// </summary>
    /// <param name="DirectoryPath">The directory in which to look</param>
    /// <param name="FullPathName">
    ///     The name of the shortcut (without the .lnk extension) or the full path to a file of the same
    ///     name
    /// </param>
    /// <returns>Returns true if the link exists</returns>
    public static bool Exists(string DirectoryPath, string LinkPathName)
    {
        // Get some file and directory information
        var SpecialDir = new DirectoryInfo(DirectoryPath);
        // First get the filename for the original file and create a new file
        // name for a link in the Startup directory
        //
        var originalfile = new FileInfo(LinkPathName);
        var NewFileName  = SpecialDir.FullName + "\\" + originalfile.Name + ".lnk";
        var linkfile     = new FileInfo(NewFileName);
        return linkfile.Exists;
    }

    //Check to see if a shell link exists to the given path in the specified special folder
    // return true if it exists
    public static bool Exists(Environment.SpecialFolder folder, string LinkPathName)
    {
        return Exists(Environment.GetFolderPath(folder), LinkPathName);
    }

    /// <summary>
    ///     Update the specified folder by creating or deleting a Shell Link if necessary
    /// </summary>
    /// <param name="folder">A SpecialFolder in which the link will reside</param>
    /// <param name="TargetPathName">The path name of the target file for the link</param>
    /// <param name="LinkPathName">
    ///     The file name for the link itself or, if a path name the directory information will be
    ///     ignored.
    /// </param>
    /// <param name="Create">If true, create the link, otherwise delete it</param>
    public static void Update(Environment.SpecialFolder folder, string TargetPathName, string LinkPathName, bool install)
    {
        // Get some file and directory information
        Update(Environment.GetFolderPath(folder), TargetPathName, LinkPathName, install);
    }

    // boolean variable "install" determines whether the link should be there or not.
    // Update the folder by creating or deleting the link as required.

    /// <summary>
    ///     Update the specified folder by creating or deleting a Shell Link if necessary
    /// </summary>
    /// <param name="DirectoryPath">The full path of the directory in which the link will reside</param>
    /// <param name="TargetPathName">The path name of the target file for the link</param>
    /// <param name="LinkPathName">
    ///     The file name for the link itself or, if a path name the directory information will be
    ///     ignored.
    /// </param>
    /// <param name="Create">If true, create the link, otherwise delete it</param>
    public static void Update(string DirectoryPath, string TargetPathName, string LinkPathName, bool Create)
    {
        // Get some file and directory information
        var SpecialDir = new DirectoryInfo(DirectoryPath);
        // First get the filename for the original file and create a new file
        // name for a link in the Startup directory
        //
        var OriginalFile = new FileInfo(LinkPathName);
        var NewFileName  = SpecialDir.FullName + "\\" + OriginalFile.Name + ".lnk";
        var LinkFile     = new FileInfo(NewFileName);

        if (Create) // If the link doesn't exist, create it
        {
            if (LinkFile.Exists)
            {
                return; // We're all done if it already exists
            }

            //Place a shortcut to the file in the special folder 
            try
            {
                // Create a shortcut in the special folder for the file
                // Making use of the Windows Scripting Host
                var shell = new WshShell();
                var link  = (IWshShortcut) shell.CreateShortcut(LinkFile.FullName);
                link.TargetPath = TargetPathName;
                link.Save();
            }
            catch
            {
                MessageBox.Show("Unable to create link in special directory: " + NewFileName, "Shell Link Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else // otherwise delete it from the startup directory
        {
            if (!LinkFile.Exists)
            {
                return; // It doesn't exist so we are done!
            }

            try
            {
                LinkFile.Delete();
            }
            catch
            {
                MessageBox.Show("Error deleting link in special directory: " + NewFileName, "Shell Link Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}