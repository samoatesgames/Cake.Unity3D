using System;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Core;
using System.IO;
using System.Text;

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
        /// <param name="projectFolders"></param>
        [CakeMethodAlias]
        public static void Build(this ICakeContext context, FilePath projectFolders)
        {

        }
    }
}

