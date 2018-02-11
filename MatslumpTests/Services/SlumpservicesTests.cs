using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matslump.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matslump.Models;

namespace Matslump.Services.Tests
{
    [TestClass()]
    public class SlumpservicesTests
    {
        [TestMethod()]
        public void CreateRandomListOfReceptTest()
        {
            var Test = new Slumpservices();
            var List = new List<Receptmodels>();
            List = Test.CreateRandomListOfRecept();

            Assert.IsNotNull(List);
        }

    }
}