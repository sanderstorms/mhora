using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace mhora.Util
{
    public static class Dll
    {
        /// <summary>
        /// Maps the specified executable module into the address space of the calling process.
        /// </summary>
        /// <param name="dllname">The name of the dll</param>
        /// <returns>The handle to the library</returns>
        public static IntPtr LoadLibrary(string dllname)
        {
            const int loadLibrarySearchDllLoadDir = 0x00000100;
            const int loadLibrarySearchDefaultDirs = 0x00001000;
            //const int loadLibrarySearchUserDirs = 0x00000400;
            IntPtr handler = LoadLibraryEx(dllname, IntPtr.Zero, loadLibrarySearchDllLoadDir | loadLibrarySearchDefaultDirs);
            //IntPtr handler = LoadLibraryEx(dllname, IntPtr.Zero, loadLibrarySearchUserDirs);
            if (handler == IntPtr.Zero)
            {
                int error = Marshal.GetLastWin32Error();

                System.ComponentModel.Win32Exception ex = new System.ComponentModel.Win32Exception(error);
                Trace.WriteLine(string.Format("LoadLibraryEx {0} failed with error code {1}: {2}", dllname, (uint)error, ex.Message));
                if (error == 5)
                {
                    Trace.WriteLine(string.Format("Please check if the current user has execute permission for file: {0} ", dllname));
                }
            }
            return handler;
        }


        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibraryEx(
            [MarshalAs(UnmanagedType.LPStr)]
            string fileName,
            IntPtr hFile,
            int dwFlags);

        /// <summary>
        /// Decrements the reference count of the loaded dynamic-link library (DLL). When the reference count reaches zero, the module is unmapped from the address space of the calling process and the handle is no longer valid
        /// </summary>
        /// <param name="handle">The handle to the library</param>
        /// <returns>If the function succeeds, the return value is true. If the function fails, the return value is false.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(IntPtr handle);

        const uint LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDefaultDllDirectories(uint DirectoryFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int AddDllDirectory(string NewDirectory);

        public static bool LoadUnmanagedModule(string module)
        {
            var loadDirectory = mhora.WorkingDir;
            var subfolder = IntPtr.Size == 8 ? "x64" : "x86";

            if (!string.IsNullOrEmpty(subfolder))
            {
                var temp = Path.Combine(loadDirectory, subfolder);
                if (Directory.Exists(temp))
                {
                    loadDirectory = temp;
                }
                else
                {
                    loadDirectory = Path.Combine(Path.GetFullPath("."), subfolder);
                }
            }

            string oldDir = Environment.CurrentDirectory;
            if (!string.IsNullOrEmpty(loadDirectory) && Directory.Exists(loadDirectory))
                Environment.CurrentDirectory = loadDirectory;

            Debug.WriteLine(string.Format("Loading open cv binary from {0}", loadDirectory));
            bool success = true;

            //Use absolute path for Windows Desktop
            var fullPath = Path.Combine(loadDirectory, module);

            bool fileExist = File.Exists(fullPath);
            if (!fileExist)
            {
                Trace.WriteLine(string.Format("File {0} do not exist.", fullPath));
            }
            bool fileExistAndLoaded = fileExist && !IntPtr.Zero.Equals(LoadLibrary(fullPath));
            if (fileExist && (!fileExistAndLoaded))
            {
                Trace.WriteLine(string.Format("File {0} cannot be loaded.", fullPath));
            }
            success &= fileExistAndLoaded;

            Environment.CurrentDirectory = oldDir;

            return success;
        }
    }
}
