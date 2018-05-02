using System;
using Xunit;
using Cake.Core.IO;

namespace Cake.Unity3D.Tests
{
    public class Unity3DTests : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly FakeCakeContext m_context = new FakeCakeContext();

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            m_context.DumpLogs();
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void Example()
        {
            Assert.Null(null);
        }
    }
}

