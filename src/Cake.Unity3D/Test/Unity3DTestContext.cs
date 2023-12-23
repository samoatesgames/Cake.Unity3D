using Cake.Core;
using Cake.Core.IO;
using Cake.Unity3D.Helpers;
using System;
using System.Diagnostics;
using System.Xml.Linq;
using Cake.Common.Diagnostics;

namespace Cake.Unity3D
{
    /// <summary>
    /// The core test context for the a Unity3D project.
    /// </summary>
    public class Unity3DTestContext
    {
        /// <summary>
        /// The cake context being used.
        /// </summary>
        private readonly ICakeContext m_cakeContext;

        /// <summary>
        /// The absolute path to the Unity3D project to test.
        /// </summary>
        private readonly FilePath m_projectFolder;

        /// <summary>
        /// The test options to use when building the project.
        /// </summary>
        private readonly Unity3DTestOptions m_testOptions;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">The current cake context.</param>
        /// <param name="projectFolder">The absolute path to the Unity3D project to build.</param>
        /// <param name="options">The test options to use when building the project.</param>
        public Unity3DTestContext(ICakeContext context, FilePath projectFolder, Unity3DTestOptions options)
        {
            m_cakeContext = context;
            m_projectFolder = projectFolder;
            m_testOptions = options;

            if (string.IsNullOrEmpty(options.TestResultOutputPath))
            {
                options.TestResultOutputPath = System.IO.Path.Combine(projectFolder.FullPath, "test_results.xml");
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
            Console.WriteLine($"TestMode: {m_testOptions.TestMode}");
            Console.WriteLine($"TestResultOutputPath: {m_testOptions.TestResultOutputPath}");
            Console.WriteLine($"OutputEditorLog: {m_testOptions.OutputEditorLog}");
            Console.WriteLine($"UnityEditorLocation: {m_testOptions.UnityEditorLocation}");
        }

        /// <summary>
        /// Perform a build using the contexts project directory and build options.
        /// </summary>
        public void Test()
        {
            var testArguments =
                "-batchmode " +
                "-runTests " +
                "-nographics " +
                $"-projectPath \"{m_projectFolder.FullPath}\" " +
                $"-testPlatform {m_testOptions.TestMode.ToString().ToLower()} " +
                $"-testResults \"{m_testOptions.TestResultOutputPath}\"";

            if (System.IO.File.Exists(m_testOptions.TestResultOutputPath))
            {
                try
                {
                    System.IO.File.Delete(m_testOptions.TestResultOutputPath);
                }
                catch (Exception e)
                {
                    throw new Exception($"Failed to delete the existing test results file from '{m_testOptions.TestResultOutputPath}'", e);
                }
            }

            // Create the process using the Unity editor and arguments above.
            using (var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = m_testOptions.UnityEditorLocation,
                    Arguments = testArguments,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            })
            {
                Console.WriteLine($"Running: \"{m_testOptions.UnityEditorLocation}\" {testArguments}");

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
                        m_testOptions.OutputEditorLog,
                        logLocation,
                        ref outputLineIndex);
                }

                if (logReportedError)
                {
                    throw new Exception("An error was reported in the Unity3D editor log.");
                }
            }

            GenerateTestResultsOverview();
        }

        private void GenerateTestResultsOverview()
        {
            if (!System.IO.File.Exists(m_testOptions.TestResultOutputPath))
            {
                throw new Exception($"No test result file exists at '{m_testOptions.TestResultOutputPath}' to generate a report from.");
            }

            var document = XDocument.Load(m_testOptions.TestResultOutputPath);
            var testRunElement = document.Element("test-run");
            if (testRunElement == null)
            {
                throw new Exception($"Badly formatted test results exist within '{m_testOptions.TestResultOutputPath}', root 'test-run' element does not exist.");
            }

            if (!int.TryParse(testRunElement.Attribute("total")?.Value, out var totalTestRuns))
            {
                throw new Exception($"Badly formatted test results exist within '{m_testOptions.TestResultOutputPath}', 'total' attribute on 'test-run' element does not exist.");
            }

            if (!int.TryParse(testRunElement.Attribute("passed")?.Value, out var passedTestRuns))
            {
                throw new Exception($"Badly formatted test results exist within '{m_testOptions.TestResultOutputPath}', 'passed' attribute on 'test-run' element does not exist.");
            }

            if (!int.TryParse(testRunElement.Attribute("failed")?.Value, out var failedTestRuns))
            {
                throw new Exception($"Badly formatted test results exist within '{m_testOptions.TestResultOutputPath}', 'failed' attribute on 'test-run' element does not exist.");
            }

            if (!int.TryParse(testRunElement.Attribute("inconclusive")?.Value, out var inconclusiveTestRuns))
            {
                throw new Exception($"Badly formatted test results exist within '{m_testOptions.TestResultOutputPath}', 'inconclusive' attribute on 'test-run' element does not exist.");
            }

            if (!int.TryParse(testRunElement.Attribute("skipped")?.Value, out var skippedTestRuns))
            {
                throw new Exception($"Badly formatted test results exist within '{m_testOptions.TestResultOutputPath}', 'skipped' attribute on 'test-run' element does not exist.");
            }
            
            m_cakeContext.Information("");
            m_cakeContext.Information("========================================");
            m_cakeContext.Information("Test Results Report");
            m_cakeContext.Information("========================================");
            m_cakeContext.Information($"- Total: {totalTestRuns}");
            m_cakeContext.Information($"- Passed: {passedTestRuns}");
            m_cakeContext.Information($"- Failed: {failedTestRuns}");
            m_cakeContext.Information($"- Inconclusive: {inconclusiveTestRuns}");
            m_cakeContext.Information($"- Skipped: {skippedTestRuns}");
            m_cakeContext.Information("");

            if (failedTestRuns != 0)
            {
                throw new Exception($"A total of '{failedTestRuns}' of a possible '{totalTestRuns}' have failed");
            }
        }
    }
}
