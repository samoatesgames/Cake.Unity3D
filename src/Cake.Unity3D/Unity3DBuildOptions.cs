namespace Cake.Unity3D
{
    /// <summary>
    /// All build options available when performing a Unity3D build.
    /// </summary>
    public class Unity3DBuildOptions
    {
        /// <summary>
        /// The platform to build for.
        /// </summary>
        public Unity3DBuildPlatform Platform { get; set; }

        /// <summary>
        /// The target path for the build project.
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// The location of the Unity.exe to use.
        /// </summary>
        public string UnityEditorLocation { get; set; }
    }
}
