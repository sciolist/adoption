using Adoption.Configuration;
using NUnit.Framework;

namespace Adoption.Tests
{
    [TestFixture]
    public class WhenScanningStrings
    {
        [Test]
        public void CanGetRemainingMatch()
        {
            var scanner = new StringScanner("foobar");
            Assert.That(scanner.Remaining, Is.EqualTo("foobar"));
        }

        [Test]
        public void IncrementsOnMatch()
        {
            var scanner = new StringScanner("foobar");
            scanner.Scan("^foo");
            Assert.That(scanner.Remaining, Is.EqualTo("bar"));
        }

        [Test]
        public void DoesntIncrementWithoutMatch()
        {
            var scanner = new StringScanner("foobar");
            scanner.Scan("^bar");
            Assert.That(scanner.Remaining, Is.EqualTo("foobar"));
        }

        [Test]
        public void CanSkipValues()
        {
            var scanner = new StringScanner("foobar");
            if (!scanner.Skip("^foo")) Assert.Fail("Skip failed!");
            Assert.That(scanner.Remaining, Is.EqualTo("bar"));
        }

        [Test]
        public void CanFailAtSkippingValues()
        {
            var scanner = new StringScanner("foobar");
            if (scanner.Skip("^bar")) Assert.Fail("Skip failed!");
        }
    }
}