using System;
using NUnit.Framework;

namespace Adoption.Tests
{
    [TestFixture]
    public class Example
    {
        [Test]
        public void CanProcess()
        {
            Show("--method", "name", "-ctest", "--full");
            Show("--method", "description", "-ctest", "--full");

            Show("--method", "name", "-ctest");
            Show("--method", "description", "-ctest");

            Assert.Throws<ValueRequiredException>(() => Show("--method", "name"));
        }

        private static void Show(params string[] args)
        {
            var processor = new Processor(defaultArgument: "--method");
            processor.Handle("--configuration", "-c").Required();
            var method = processor.Handle("--method", "-m");
            var full = processor.Handle("--full").Flag(false);
            processor.Process(args);

            var showFull = full.Enabled;
            switch(method.Value.ToString())
            {
                case "name":
                    Console.WriteLine("Name: {0}", showFull ? "Adoption Option Parser" : "adoption");
                    break;
                case "description":
                    Console.WriteLine("Description: {0}", showFull ? "Parses command line options!" : "option parser");
                    break;
            }
        }
    }
}
