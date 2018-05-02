using System;
using Cake.Core.IO;
using Cake.Core;
using Cake.Core.Tooling;
using Cake.Testing;

namespace Cake.Unity3D.Tests
{
    public class FakeCakeContext
    {
        ICakeContext context;
        FakeLog log;
        DirectoryPath testsDir;


        public FakeCakeContext ()
        {
            testsDir = new DirectoryPath(System.IO.Path.GetFullPath(AppContext.BaseDirectory));

            var environment = Cake.Testing.FakeEnvironment.CreateUnixEnvironment (false);

            var fileSystem = new Cake.Testing.FakeFileSystem (environment);
            var globber = new Globber (fileSystem, environment);
            log = new Cake.Testing.FakeLog ();
            var args = new FakeCakeArguments ();
            var processRunner = new ProcessRunner (environment, log);
            var registry = new WindowsRegistry ();

            var tools = new ToolLocator(environment, new ToolRepository(environment), new ToolResolutionStrategy(fileSystem, environment, globber, new FakeConfiguration()));
            context = new CakeContext (fileSystem, environment, globber, log, args, processRunner, registry, tools);
            context.Environment.WorkingDirectory = testsDir;
        }

        public DirectoryPath WorkingDirectory {
            get { return testsDir; }
        }

        public ICakeContext CakeContext {
            get { return context; }
        }

        public string GetLogs ()
        {
            return string.Join(Environment.NewLine, log.Entries);
        }

        public void DumpLogs ()
        {
            foreach (var m in log.Entries)
                Console.WriteLine (m);
        }
    }
}
