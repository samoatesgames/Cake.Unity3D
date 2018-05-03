using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Core;
using System.Collections.Generic;

namespace Cake.Unity3D
{
    /// <summary>
    /// Adds the ability to build Unity3D projects using cake.
    /// </summary>
    [CakeAliasCategory("Unity3D")]
    public static class Unity3DAliases
    {
        /// <summary>
        /// Build a provided Unity3D project with the specified build options.
        /// </summary>
        /// <param name="context">The active cake context.</param>
        /// <param name="projectFolder">The absolute path to the Unity3D project to build.</param>
        /// <param name="options">The build options to use when building the project.</param>
        [CakeMethodAlias]
        public static void BuildUnity3DProject(this ICakeContext context, FilePath projectFolder, Unity3DBuildOptions options)
        {
            var unityBuildContext = new Unity3DBuildContext(context, projectFolder, options);
            unityBuildContext.DumpOptions();
            unityBuildContext.Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [CakeMethodAlias]
        public static Dictionary<string, string> GetAllUnityInstalls(this ICakeContext context)
        {
            return Helpers.Unity3DEditor.LocateUnityInstalls();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="version"></param>
        /// <param name="installPath"></param>
        /// <returns></returns>
        [CakeMethodAlias]
        public static bool TryGetUnityInstall(this ICakeContext context, string version, out string installPath)
        {
            var installs = context.GetAllUnityInstalls();
            return installs.TryGetValue(version, out installPath);
        }
    }
}

