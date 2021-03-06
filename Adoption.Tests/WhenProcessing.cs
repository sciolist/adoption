﻿using System;
using NUnit.Framework;

namespace Adoption.Tests
{
    [TestFixture]
    public class WhenProcessing
    {
        [Test]
        public void CanSpecifyDefaultArgument()
        {
            var proc = new Processor(defaultArgument: "--test");
            proc.Process(new[] { "foo" });
            Assert.That(proc["--test"], Is.EqualTo("foo"));
        }

        [Test]
        public void CanGetUnresolvedParts()
        {
            var proc = new Processor();
            proc.Process(new[] { "--test1", "foo" });
            Assert.AreEqual(new[] {"--test1", "foo" }, proc.UnresolvedParts);
        }

        [Test]
        public void CanDisplayHelp()
        {
            const string text = "Supplies a value for tests.";
            var proc = new Processor(defaultArgument: "--test");
            proc.Handle("--test").Describe(text);
            Assert.That(proc.Help(), Is.StringContaining(text));
        }

        [Test]
        public void CanAddOptionsToDefaultArgument()
        {
            var proc = new Processor(defaultArgument: "--test");
            var test = proc.Handle("--test").TakesManyValues();
            var set = new[] { "foo", "bar" };
            proc.Process(set);

            Assert.That(test.Value, Is.EquivalentTo(set));
        }

        [Test]
        public void CanImplyFlagWhenPassingMultipleKeys()
        {
            var proc = new Processor(defaultArgument: "--test");
            var test = proc.Handle("--test");
            var set = new[] { "--bar", "--test", "foo" };
            proc.Process(set);

            Assert.That(test.Value, Is.EqualTo("foo"));
        }

        [Test]
        public void CrashesOnUnassignedRequiredValues()
        {
            var proc = new Processor(defaultArgument: "--test");
            proc.Handle("--test").Required();
            Assert.Throws<ValueRequiredException>(
                () => proc.Process(new string[0])
                );
        }
    }
}
