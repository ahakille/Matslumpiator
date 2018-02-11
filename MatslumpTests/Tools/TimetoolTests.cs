using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matslump.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matslump.Tools.Tests
{
    [TestClass()]
    public class TimetoolTests
    {
        [TestMethod()]
        public void TranslateDayOfWeekTest()
        {
            var expected = "Måndag";
            var translate = "Monday";
            var actual =Timetool.TranslateDayOfWeek(translate);

            Assert.AreEqual(expected, actual);
        }
    }
}