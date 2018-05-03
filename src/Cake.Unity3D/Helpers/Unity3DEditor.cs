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
        /// Gets the absolute path to the Unity3D editor log.
        /// This presumes that Unity3D is running as a user and not a service.
        /// </summary>
        /// <returns>The absolute path to the Unity3D editor log.</returns>
        public static string GetEditorLogLocation()
        {
            var localAppdata = Environment.GetEnvironmentVariable("LocalAppData");
            return System.IO.Path.Combine(localAppdata, "Unity", "Editor", "Editor.log");
        }

        /// <summary>
        /// Output all new log lines to the console for the specified log.
        /// </summary>
        /// <param name="logLocation">The location of the log file to redirect.</param>
        /// <param name="currentLine">The line of the log of which we have already redirected.</param>
        /// <returns></returns>
        public static int OutputLogToConsole(string logLocation, int currentLine)
        {
            // The log doesn't exist, so we can't output its contents
            // to the console.
            if (!System.IO.File.Exists(logLocation))
            {
                return currentLine;
            }

            // Read all lines from the log.
            var lines = SafeReadAllLines(logLocation);

            // If the log output is less that what we have already
            // redirected, return what we have outputted presuming
            // something went wrong when reading the file.
            if (lines.Length <= currentLine)
            {
                return currentLine;
            }

            // Output all new lines of the log.
            foreach (var line in lines.Skip(currentLine))
            {
                Console.WriteLine(line);
            }

            // Return the new number of lines we have redirected.
            return lines.Length;
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
