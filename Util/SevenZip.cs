using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Mhora.Util;

public static class SevenZip
{
    private const string exe7ZipPath = @"C:\Program Files\7-Zip\7z.exe";

    // function for compressing all items within the mentioned directory(path)
    public static async Task CompressDirectoryItems(string destination_Path, string folderToCompress, string zipName, string password = "")
    {
        // 7zip application path 
        try
        {
            //condtional selection of switches based on default argument corresponding to password
            var currentCommand = password.Length > 0 ? $"a  -p{password}" + " -sdel -mhe=on -y \"" : "a " + " -sdel -y \"";
            // creating command line string  that is used to pass argument to 7zip process 
            var commandLineString = currentCommand + destination_Path + zipName + "\" \"" + folderToCompress + @"*";

            await RunProcessAsync(exe7ZipPath, commandLineString);
        }
        catch (Exception ex)
        {
            mhora.Log.Exception(ex);
        }
    }

    public static async Task ExtractZip(string zipPath, string extractpath)
    {
        try
        {
            var commandLineString = "x  -o" + extractpath + " \"" + zipPath + @"""";
            await RunProcessAsync(exe7ZipPath, commandLineString);
        }
        catch (Exception ex)
        {
            mhora.Log.Exception(ex);
        }
    }

    private static Task<int> RunProcessAsync(string fileName, string arguments, ProcessWindowStyle windowStyle = ProcessWindowStyle.Hidden)
    {
        var tcs = new TaskCompletionSource<int>();

        var process = new Process
        {
            StartInfo =
            {
                FileName    = fileName,
                Arguments   = arguments,
                WindowStyle = windowStyle
            },
            EnableRaisingEvents = true
        };

        process.Exited += (sender, args) =>
        {
            tcs.SetResult(process.ExitCode);
            process.Dispose();
        };

        process.Start();

        return tcs.Task;
    }
}