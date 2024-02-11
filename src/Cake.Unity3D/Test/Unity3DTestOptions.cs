namespace Cake.Unity3D.Test
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
}
