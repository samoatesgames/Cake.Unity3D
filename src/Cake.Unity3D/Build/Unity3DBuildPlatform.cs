namespace Cake.Unity3D
{
    /// <summary>
    /// All support build target platforms.
    /// </summary>
    public enum Unity3DBuildPlatform
    {
        /// <summary>
        /// Build a standalone windows x64 build.
        /// </summary>
        StandaloneWindows64,

        /// <summary>
        /// Build a standalone windows x86 build.
        /// </summary>
        StandaloneWindows,

        /// <summary>
        /// Build a WebGL build.
        /// </summary>
        WebGL,

        /// <summary>
        /// Build an Android build.
        /// </summary>
        Android,

        /// <summary>
        /// Build an iOS build.
        /// </summary>
        iOS,
    }
}
