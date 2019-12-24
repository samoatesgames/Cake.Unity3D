using Cake.Common.Diagnostics;
using Cake.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Unity3D.Helpers
{
    /// <summary>
    /// Some helper methods for interacting with Unity3D.
    /// </summary>
    public static class Unity3DEditor
    {
        /// <summary>
        /// Locate all installed version of Unity3D.
        /// Warning: This currently only works for Windows and has only been tested on Windows 10.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> LocateUnityInstalls()
        {
            var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            if (!System.IO.Directory.Exists(programData))
            {
                throw new Exception($"Failed to find any installed Unity3d versions. 'ProgramData' folder '{programData}' does not exist.");
            }

            var startMenuProgramsDirectory = System.IO.Path.Combine(programData, "Microsoft", "Windows", "Start Menu", "Programs");
            if (!System.IO.Directory.Exists(startMenuProgramsDirectory))
            {
                throw new Exception($"Failed to find any installed Unity3d versions. Start menu programs folder '{startMenuProgramsDirectory}' does not exist.");
            }

            var installs = new Dictionary<string, string>();
            foreach (var unityFolder in System.IO.Directory.EnumerateDirectories(startMenuProgramsDirectory, "Unity*"))
            {
                var unityShortcut = System.IO.Path.Combine(unityFolder, "Unity.lnk");
                if (!System.IO.File.Exists(unityShortcut))
                {
                    continue;
                }

                installs.Add(new System.IO.DirectoryInfo(unityFolder).Name, WindowsShortcut.GetShortcutTarget(unityShortcut));
            }
            return installs;
        }

        /// <summary>
        /// Gets the absolute path to the Unity3D editor log.
        /// This presumes that Unity3D is running as a user and not a service.
        /// </summary>
        /// <returns>The absolute path to the Unity3D editor log.</returns>
        public static string GetEditorLogLocation()
        {
            var localAppdata = Environment.GetEnvironmentVariable("LocalAppData");
            if (string.IsNullOrEmpty(localAppdata))
            {
                throw new Exception("Failed to find the 'LocalAppData' directory.");
            }

            return System.IO.Path.Combine(localAppdata, "Unity", "Editor", "Editor.log");
        }

        /// <summary>
        /// Output all new log lines to the console for the specified log.
        /// </summary>
        /// <param name="context">The active cake context.</param>
        /// <param name="outputEditorLog">Should the log be forward to the standard cake context output.</param>
        /// <param name="logLocation">The location of the log file to redirect.</param>
        /// <param name="currentLine">The line of the log of which we have already redirected.</param>
        /// <returns></returns>
        public static bool ProcessEditorLog(ICakeContext context, bool outputEditorLog, string logLocation, ref int currentLine)
        {
            // The log doesn't exist, so we can't output its contents
            // to the console.
            if (!System.IO.File.Exists(logLocation))
            {
                return false;
            }

            // Read all lines from the log.
            var lines = SafeReadAllLines(logLocation);

            // If the log output is less that what we have already
            // redirected, return what we have outputted presuming
            // something went wrong when reading the file.
            if (lines.Length <= currentLine)
            {
                return false;
            }

            var hasError = false;

            // Output all new lines of the log.
            foreach (var line in lines.Skip(currentLine))
            {
                var logType = Unity3DEditorLog.ProcessLogLine(line);
                if (outputEditorLog)
                {
                    switch (logType)
                    {
                        case Unity3DEditorLog.MessageType.Debug:
                            context.Debug(line);
                            break;
                        case Unity3DEditorLog.MessageType.Info:
                            context.Information(line);
                            break;
                        case Unity3DEditorLog.MessageType.Warning:
                            context.Warning(line);
                            break;
                        case Unity3DEditorLog.MessageType.Error:
                            context.Error(line);
                            hasError = true;
                            break;
                    }
                }
                else if (logType == Unity3DEditorLog.MessageType.Error)
                {
                    hasError = true;
                }
            }

            // Return the new number of lines we have redirected.
            currentLine = lines.Length;
            return hasError;
        }

        /// <summary>
        /// Read all lines from a given file.
        /// Using read/write file share access.
        /// </summary>
        /// <param name="path">The absolute path to the file to read.</param>
        /// <returns>An array of all lines in the log file.</returns>
        private static string[] SafeReadAllLines(string path)
        {
            try
            {
                using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
                {
                    using (var reader = new System.IO.StreamReader(fileStream))
                    {
                        var file = new List<string>();
                        while (!reader.EndOfStream)
                        {
                            file.Add(reader.ReadLine());
                        }
                        return file.ToArray();
                    }
                }
            }
            catch (Exception)
            {
                // Something went wrong, return an empty array
                return new string[0];
            }
        }
    }
}
