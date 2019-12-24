using NUnit.Framework;

namespace Tests
{
    public class ExampleTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ExampleTestSimplePasses()
        {
            Assert.IsTrue(true);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ExampleTestSimpleFails()
        {
            Assert.IsTrue(true);
        }
    }
}
