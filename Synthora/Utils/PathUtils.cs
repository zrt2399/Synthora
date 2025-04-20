using System;
using System.Diagnostics;
using System.IO;

namespace Synthora.Utils
{
    internal static class PathUtils
    {
        public static string GetDirectoryName(this string path)
        {
            return Path.GetDirectoryName(path) ?? path;
        }

        public static Process? OpenFileLocation(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath))
            {
                return null;
            }
            if (OperatingSystem.IsWindows())
            {
                return Process.Start("explorer.exe", $"/select,\"{fullPath}\"");
            }
            else if (OperatingSystem.IsLinux())
            {
                var folderPath = GetDirectoryName(fullPath);
                return Process.Start("xdg-open", $"\"{folderPath}\"");
            }
            else
            {
                return null;
            }
        }

        public static Process? OpenFlie(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }
            ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName);
            //processStartInfo.WorkingDirectory = GetDirectoryName(fileName);
            Process process = new Process();
            process.StartInfo = processStartInfo;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.ErrorDialog = true;
            process.Start();
            return process;
        }
    }
}