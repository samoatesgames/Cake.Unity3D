using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Core;

namespace Cake.Unity3D
{
    /// <summary>
    /// 
    /// </summary>
    [CakeAliasCategory ("Unity3D")]
    public static class Unity3DAliases
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="projectFolder"></param>
        /// <param name="options"></param>
        [CakeMethodAlias]
        public static void BuildUnity3DProject(this ICakeContext context, FilePath projectFolder, Unity3DBuildOptions options)
        {
            var unityBuildContext = new Unity3DBuildContext(context, projectFolder, options);
            unityBuildContext.DumpOptions();
            unityBuildContext.Build();
        }
    }
}

