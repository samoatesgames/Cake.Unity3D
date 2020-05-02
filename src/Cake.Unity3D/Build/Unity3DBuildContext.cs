using System;
using System.Diagnostics;
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

            if (string.IsNullOrEmpty(options.OutputPath))
            {
                throw new Exception("The output path build option must be set.");
            }

            if (options.BuildVersion != null && options.BuildVersion.Contains(" "))
            {
                throw new Exception("The build version can not contain any spaces.");
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
            Console.WriteLine($"OutputPath: \"{m_buildOptions.OutputPath}\"");
            Console.WriteLine($"BuildVersion: {m_buildOptions.BuildVersion}");
            Console.WriteLine($"UnityEditorLocation: {m_buildOptions.UnityEditorLocation}");
            Console.WriteLine($"OutputEditorLog: {m_buildOptions.OutputEditorLog}");
            Console.WriteLine($"ForceScriptInstall: {m_buildOptions.ForceScriptInstall}");
        }

        /// <summary>
        /// Perform a build using the contexts project directory and build options.
        /// </summary>
        public void Build()
        {
            // Make sure the automated build script has been copied to the Unity project.
            // The build script is a Unity script that actually invokes the build.
            if (!ProjectHasAutomatedBuildScript() || m_buildOptions.ForceScriptInstall)
            {
                InstallAutomatedBuildScript();
            }

            // The command line arguments to use.
            // All options which start with duel hyphens are used internally by
            // the automated build script.
            var buildArguments =
                "-batchmode " +
                "-quit " +
                $"-projectPath \"{m_projectFolder.FullPath}\" " +
                "-executeMethod Cake.Unity3D.AutomatedBuild.Build " +
                $"--output-path=\"{m_buildOptions.OutputPath}\" " +
                $"--platform={m_buildOptions.Platform} ";

            if (!string.IsNullOrEmpty(m_buildOptions.BuildVersion))
            {
                buildArguments += $"--version={m_buildOptions.BuildVersion} ";
            }

            // Create the process using the Unity editor and arguments above.
            using (var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = m_buildOptions.UnityEditorLocation,
                    Arguments = buildArguments,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            })
            {
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

                // Will be set to true if an error is detected within the Unity editor log.
                var logReportedError = false;

                // Whilst the process is still running, periodically redirect the editor log
                // to the console if required.
                while (!process.HasExited)
                {
                    System.Threading.Thread.Sleep(100);
                    logReportedError |= Unity3DEditor.ProcessEditorLog(
                        m_cakeContext, 
                        m_buildOptions.OutputEditorLog, 
                        logLocation,
                        ref outputLineIndex);
                }

                if (logReportedError)
                {
                    throw new Exception("An error was reported in the Unity3D editor log.");
                }
            }
        }

        /// <summary>
        /// Gets the path to the automated build script within a provided Unity3D project path.
        /// </summary>
        /// <returns>The absolute path to the automated build script.</returns>
        private string GetAutomatedBuildScriptPath()
        {
            return System.IO.Path.Combine(m_projectFolder.FullPath, "Assets", "Cake.Unity3D", "Editor", "AutomatedBuild.cs");
        }

        /// <summary>
        /// Checks to see if the provided project has the automated build script already.
        /// </summary>
        /// <returns>True if the build script already exists.</returns>
        private bool ProjectHasAutomatedBuildScript()
        {
            return System.IO.File.Exists(GetAutomatedBuildScriptPath());
        }

        /// <summary>
        /// Extract the embedded automated build script resource to the Unity3D project.
        /// </summary>
        private void InstallAutomatedBuildScript()
        {
            Console.WriteLine("Installing AutomatedBuild Script...");

            var path = GetAutomatedBuildScriptPath();

            // Make sure the directories required for the script exist.
            var installDirectory = System.IO.Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(installDirectory) && !System.IO.Directory.Exists(installDirectory))
            {
                System.IO.Directory.CreateDirectory(installDirectory);
            }

            // Extract the embedded resource to the Unity3D project provided.
            var resourceName = "Cake.Unity3D.Resources.AutomatedBuild.template";
            using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (resource == null)
                {
                    throw new Exception($"Failed to find the embedded resource '{resourceName}'");
                }

                using (var file = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }

            Console.WriteLine($"AutomatedBuild Script installed to \"{path}\"");
        }
    }
}
