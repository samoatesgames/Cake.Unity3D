using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Unity3D
{
    /// <summary>
    /// 
    /// </summary>
    public class Unity3DBuildContext
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICakeContext m_cakeContext;

        /// <summary>
        /// 
        /// </summary>
        private readonly FilePath m_projectFolder;

        /// <summary>
        /// 
        /// </summary>
        private readonly Unity3DBuildOptions m_buildOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="projectFolder"></param>
        /// <param name="options"></param>
        public Unity3DBuildContext(ICakeContext context, FilePath projectFolder, Unity3DBuildOptions options)
        {
            m_cakeContext = context;
            m_projectFolder = projectFolder;
            m_buildOptions = options;

            if (options.OutputPath.Contains(" "))
            {
                throw new Exception("The output path can not contain any spaces.");
            }

            if (!System.IO.File.Exists(options.UnityEditorLocation))
            {
                throw new Exception($"The Unity Editor location '{options.UnityEditorLocation}' does not exist.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void DumpOptions()
        {
            Console.WriteLine($"ProjectPlatform: {m_projectFolder}");
            Console.WriteLine($"Platform: {m_buildOptions.Platform}");
            Console.WriteLine($"OutputPath: {m_buildOptions.OutputPath}");
            Console.WriteLine($"UnityEditorLocation: {m_buildOptions.UnityEditorLocation}");
        }

        /// <summary>
        /// 
        /// </summary>
        public void Build()
        {
            if (!ProjectHasAutomatedBuildScript(m_projectFolder.FullPath))
            {
                InstallAutomatedBuildScript(m_projectFolder.FullPath);
            }

            var buildArguments = 
                "-batchmode " +
                "-quit " +
                $"-projectPath \"{m_projectFolder.FullPath}\" " +
                $"-executeMethod Cake.Unity3D.AutomatedBuild.Build " +
                $"--output-path={m_buildOptions.OutputPath} " +
                $"--platform={m_buildOptions.Platform} ";

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = m_buildOptions.UnityEditorLocation,
                    Arguments = buildArguments,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };

            Console.WriteLine($"Running: \"{m_buildOptions.UnityEditorLocation}\" {buildArguments}");

            process.Start();

            var outputLineIndex = 0;
            var logLocation = LocatedEditorLog();
            while (!process.HasExited)
            {
                System.Threading.Thread.Sleep(100);
                outputLineIndex = OutputLogToConsole(logLocation, outputLineIndex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string LocatedEditorLog()
        {
            var localAppdata = Environment.GetEnvironmentVariable("LocalAppData");
            return System.IO.Path.Combine(localAppdata, "Unity", "Editor", "Editor.log");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logLocation"></param>
        /// <param name="currentLine"></param>
        /// <returns></returns>
        private int OutputLogToConsole(string logLocation, int currentLine)
        {
            if (!System.IO.File.Exists(logLocation))
            {
                return currentLine;
            }

            var lines = SafeReadAllLines(logLocation);
            foreach (var line in lines.Skip(currentLine))
            {
                Console.WriteLine(line);
            }
            return lines.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] SafeReadAllLines(string path)
        {
            using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
            {
                using (var sr = new System.IO.StreamReader(fileStream))
                {
                    var file = new List<string>();
                    while (!sr.EndOfStream)
                    {
                        file.Add(sr.ReadLine());
                    }
                    return file.ToArray();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectDirectory"></param>
        /// <returns></returns>
        private string GetAutomatedBuildScriptPath(string projectDirectory)
        {
            return System.IO.Path.Combine(m_projectFolder.FullPath, "Assets", "Cake.Unity3D", "Editor", "AutomatedBuild.cs");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectDirectory"></param>
        /// <returns></returns>
        private bool ProjectHasAutomatedBuildScript(string projectDirectory)
        {
            return System.IO.File.Exists(GetAutomatedBuildScriptPath(projectDirectory));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectDirectory"></param>
        private void InstallAutomatedBuildScript(string projectDirectory)
        {
            Console.WriteLine("Installing AutomatedBuild Script...");

            var path = GetAutomatedBuildScriptPath(projectDirectory);

            var installDirectory = System.IO.Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(installDirectory) && !System.IO.Directory.Exists(installDirectory))
            {
                System.IO.Directory.CreateDirectory(installDirectory);
            }

            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("Cake.Unity3D.Resources.AutomatedBuild.template"))
            {
                using (var file = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }

            Console.WriteLine($"AutomatedBuild Script installed to \"{path}\"");
        }
    }
}
