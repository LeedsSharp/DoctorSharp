using System;
using System.IO;
using DrSharp.Domain.Logic;
using NUnit.Framework;

namespace DrSharp.Tests
{
    [TestFixture]
    public class DoctorSharpTests
    {
        #region Tests

        [Test]
        public void Can_load_settings()
        {
            var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"));
            path = Path.Combine(path, Path.Combine("config", "Settings.xml"));
            var doctorSharp = new DoctorSharp(path);
            Assert.DoesNotThrow(() => doctorSharp.Ask("unit test", "Are you green?"));
        }

        [Test]
        public void Ask_returns_answer()
        {
            var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"));
            path = Path.Combine(path, Path.Combine("config", "Settings.xml"));
            var doctorSharp = new DoctorSharp(path);
            const string expectedAnswer = "I am transparent--software has no color.";

            var answer = doctorSharp.Ask("unit test", "Are you green?");

            Assert.AreEqual(expectedAnswer, answer);
        }
        #endregion
    }
}