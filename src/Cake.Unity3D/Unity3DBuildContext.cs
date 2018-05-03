using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.IO;
using Cake.Unity3D.Helpers;

namespace Cake.Unity3D
{
    /// <summary>
    /// The core build context for the a Unity3D project.
    /// </summary>
    public class Unity3DBuildContext
    {
        /// <summary>
        /// The cake context being used.
        /// </summary>
        private readonly ICakeContext m_cakeContext;

        /// <summary>
        /// The absolute path to the Unity3D project to build.
        /// </summary>
        private readonly FilePath m_projectFolder;

        /// <summary>
        /// The build options to use when building the project.
        /// </summary>
        private readonly Unity3DBuildOptions m_buildOptions;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">The current cake context.</param>
        /// <param name="projectFolder">The absolute path to the Unity3D project to build.</param>
        /// <param name="options">The build options to use when building the project.</param>
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
        /// Outputs all current options for this build context.
        /// </summary>
        public void DumpOptions()
        {
            Console.WriteLine($"ProjectPlatform: {m_projectFolder}");
            Console.WriteLine($"Platform: {m_buildOptions.Platform}");
            Console.WriteLine($"OutputPath: {m_buildOptions.OutputPath}");
            Console.WriteLine($"UnityEditorLocation: {m_buildOptions.UnityEditorLocation}");
        }

        /// <summary>
        /// Perform a build using the contexts project directory and build options.
        /// </summary>
        public void Build()
        {
            // Make sure the automated build script has been copied to the Unity project.
            // The build script is a Unity script that actually invokes the build.
            if (!ProjectHasAutomatedBuildScript(m_projectFolder.FullPath) || m_buildOptions.ForceScriptInstall)
            {
                InstallAutomatedBuildScript(m_projectFolder.FullPath);
            }

            // The command line arguments to use.
            // All options which start with duel hyphens are used internally by
            // the automated build script.
            var buildArguments = 
                "-batchmode " +
                "-quit " +
                $"-projectPath \"{m_projectFolder.FullPath}\" " +
                $"-executeMethod Cake.Unity3D.AutomatedBuild.Build " +
                $"--output-path={m_buildOptions.OutputPath} " +
                $"--platform={m_buildOptions.Platform} ";

            // Create the process using the Unity editor and arguments above.
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

            // Unity will output to a log, and not to the console.
            // So we have to periodically parse the log and redirect the output to the console.
            // We do this by storing how far through the log we have already seen and outputting
            // the remaining lines. This works because Unity flushes in full lines, so we should
            // always have a full line to output.
            var outputLineIndex = 0;
            var logLocation = Unity3DEditor.GetEditorLogLocation();

            // Start the process.
            process.Start();

            // Whilst the process is still running, periodically redirect the editor log
            // to the console if required.
            while (!process.HasExited)
            {
                System.Threading.Thread.Sleep(100);

                if (m_buildOptions.OutputEditorLog)
                {
                    outputLineIndex = Unity3DEditor.OutputLogToConsole(logLocation, outputLineIndex);
                }
            }
        }

        /// <summary>
        /// Gets the path to the automated build script within a provided Unity3D project path.
        /// </summary>
        /// <param name="projectDirectory">The absolute path to the Unity3D project which should contain the build script.</param>
        /// <returns>The absolute path to the automated build script.</returns>
        private string GetAutomatedBuildScriptPath(string projectDirectory)
        {
            return System.IO.Path.Combine(m_projectFolder.FullPath, "Assets", "Cake.Unity3D", "Editor", "AutomatedBuild.cs");
        }

        /// <summary>
        /// Checks to see if the provided project has the automated build script already.
        /// </summary>
        /// <param name="projectDirectory">The absolute path to the Unity3D project which should contain the build script.</param>
        /// <returns>True if the build script already exists.</returns>
        private bool ProjectHasAutomatedBuildScript(string projectDirectory)
        {
            return System.IO.File.Exists(GetAutomatedBuildScriptPath(projectDirectory));
        }

        /// <summary>
        /// Extract the embedded automated build script resource to the Unity3D project.
        /// </summary>
        /// <param name="projectDirectory">The absolute path to the Unity3D project which should contain the build script.</param>
        private void InstallAutomatedBuildScript(string projectDirectory)
        {
            Console.WriteLine("Installing AutomatedBuild Script...");

            var path = GetAutomatedBuildScriptPath(projectDirectory);

            // Make sure the directories required for the script exist.
            var installDirectory = System.IO.Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(installDirectory) && !System.IO.Directory.Exists(installDirectory))
            {
                System.IO.Directory.CreateDirectory(installDirectory);
            }

            // Extract the embedded resource to the Unity3D project provided.
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
