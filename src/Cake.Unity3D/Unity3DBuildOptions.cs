namespace Cake.Unity3D
{
    /// <summary>
    /// All build options available when performing a Unity3D build.
    /// </summary>
    public class Unity3DBuildOptions
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Unity3DBuildOptions()
        {
            Platform = Unity3DBuildPlatform.StandaloneWindows64;
            OutputEditorLog = true;
        }

        /// <summary>
        /// The platform to build for.
        /// Default: StandaloneWindows64
        /// </summary>
        public Unity3DBuildPlatform Platform { get; set; }

        /// <summary>
        /// The target path for the build project.
        /// Default: null
        /// </summary>
        public string OutputPath { get; set; }

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
    }
}
