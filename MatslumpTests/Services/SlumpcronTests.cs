﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matslump.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matslump.Services.Tests
{
    [TestClass()]
    public class SlumpcronTests
    {
        [TestMethod()]
        public void StartCronTest()
        {
            Slumpcron.StartCron();
            
        }
    }
}