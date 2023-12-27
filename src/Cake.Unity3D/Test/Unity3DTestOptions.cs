namespace Cake.Unity3D
{
    /// <summary>
    /// All build options available when performing a Unity3D build.
    /// </summary>
    public class Unity3DTestOptions
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Unity3DTestOptions()
        {
            TestMode = Unity3DTestMode.EditMode;
            OutputEditorLog = true;
        }

        /// <summary>
        /// The test to run.
        /// Default: EditMode
        /// </summary>
        public Unity3DTestMode TestMode { get; set; }

        /// <summary>
        /// The target path for the build project.
        /// Default: null
        /// </summary>
        public string TestResultOutputPath { get; set; }

        /// <summary>
        /// The location of the Unity.exe to use.
        /// Default: null
        /// </summary>
        public string UnityEditorLocation { get; set; }

        /// <summary>
        /// Should the editor log produced by Unity3D whilst building
        /// be output to the console.
        /// Default: true
        /// </summary>
        public bool OutputEditorLog { get; set; }
        
        /// <summary>
        /// Should the build produce a code coverage report.
        /// </summary>
        public bool EnableCodeCoverage { get; set; }
    }

    public class Unity3DCodeCoverageOptions 
    {
        /// <summary>
        /// (optional), to set the location where the coverage results and report are saved to.
        /// The default location is the project's path.
        /// </summary>
        public string CodeCoverageResultsLocation { get; set; }
        
        /// <summary>
        /// (optional) Set the location where the coverage report history is saved to.
        /// The default location is the project's path
        /// </summary>
        public string CodeCoverageResultsHistoryLocation { get; set; }
        
        /// <summary>
        /// Generate a HTML report.
        /// </summary>
        public bool GenerateHtmlReport { get; set; }
        
        /// <summary>
        /// Generate a HTML report history.
        /// </summary>
        public bool GenerateHtmlReportHistory { get; set; }
        
        /// <summary>
        /// Generate and include additional metrics in the HTML report.
        /// </summary>
        public bool GenerateAdditionalMetrics { get; set; }
        
        /// <summary>
        /// Generage coverage summary badges.
        /// </summary>
        public bool GenerateBadgeReport { get; set; }
        
        /// <summary>
        /// Generate SonarQube, Cobertura and LCOV reports.
        /// </summary>
        public bool GenerateAdditionalReports { get; set; }
        
        /// <summary>
        /// include test references to the generated coverage results
        /// and enable the Coverage by test methods section in the HTML report.
        /// This shows how each test contributes to the overall coverage. 
        /// </summary>
        public bool GenerateTestReferences { get; set; } 
        
        /// <summary>
        /// Use the project settings for the code coverage options.
        /// </summary>
        public bool UseProjectSettings { get; set; }
    }
}
