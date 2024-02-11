namespace Cake.Unity3D.Build
{
    /// <summary>
    /// All support build target platforms.
    /// </summary>
    public enum Unity3DBuildPlatform
    {
        /// <summary>
        /// Build a standalone windows x86 build.
        /// </summary>
        StandaloneWindows = 5,

        /// <summary>
        /// Build a standalone windows x64 build.
        /// </summary>
        StandaloneWindows64 = 19,

        /// <summary>
        /// Build a standalone OSX build.
        /// </summary>
        StandaloneOSX = 2,
 
        /// <summary>
        /// Build a standalone iOS build.
        /// </summary>
        iOS = 9,
        
        /// <summary>
        /// Build a standalone tvOS build.
        /// </summary>
        tvOS = 37,

        /// <summary>
        /// Build a standalone Linux x64 build.
        /// </summary>
        StandaloneLinux64 = 24,
        
        /// <summary>
        /// Build a standalone Linux Headless build.
        /// </summary>
        LinuxHeadlessSimulation = 41,
        
        /// <summary>
        /// Build a standalone Android build.
        /// </summary>
        Android = 13,

        /// <summary>
        /// Build a WebGL build.
        /// </summary>
        WebGL = 20,
    }
}
