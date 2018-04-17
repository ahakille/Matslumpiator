using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matslump.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matslump.Controllers.Tests
{
    [TestClass()]
    public class SlumpiatorControllerTests
    {

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}