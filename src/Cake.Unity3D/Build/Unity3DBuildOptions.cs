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
            ForceScriptInstall = false;
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
        /// A custom string used as the build version of the Unity3D project.
        /// This will be used as the bundle version in the built application.
        /// </summary>
        public string BuildVersion { get; set; }

        /// <summary>
        /// A number between 0 and 10000 used primarily for the Android version code.
        /// </summary>
        public string BuildVersionCode { get; set; }

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
        /// Should we install the automated build script
        /// even if we find an existing one.
        /// Default: false
        /// </summary>
        public bool ForceScriptInstall { get; set; }
    }
}
