using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Unity3D
{
    /// <summary>
    /// 
    /// </summary>
    public class Unity3DBuildContext
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICakeContext m_cakeContext;

        /// <summary>
        /// 
        /// </summary>
        private readonly FilePath m_projectFolder;

        /// <summary>
        /// 
        /// </summary>
        private readonly Unity3DBuildOptions m_buildOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="projectFolder"></param>
        /// <param name="options"></param>
        public Unity3DBuildContext(ICakeContext context, FilePath projectFolder, Unity3DBuildOptions options)
        {
            m_cakeContext = context;
            m_projectFolder = projectFolder;
            m_buildOptions = options;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DumpOptions()
        {
            Console.WriteLine($"ProjectPlatform: {m_projectFolder}");
            Console.WriteLine($"Platform: {m_buildOptions.Platform}");
            Console.WriteLine($"OutputPath: {m_buildOptions.OutputPath}");
            Console.WriteLine($"UnityEditorLocation: {m_buildOptions.UnityEditorLocation}");
        }

        /// <summary>
        /// 
        /// </summary>
        public void Build()
        {

        }
    }
}
