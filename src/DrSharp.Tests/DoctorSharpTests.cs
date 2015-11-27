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
            var bot = new DoctorSharp(path);
            Assert.DoesNotThrow(() => bot.AskMeAnything("unit test", "are you green"));
            
        }
        #endregion
    }
}