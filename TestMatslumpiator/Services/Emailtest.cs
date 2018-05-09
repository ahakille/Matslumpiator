using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matslumpiator.Services;
using Microsoft.Extensions.Configuration;

namespace TestMatslumpiator
{
    [TestClass]
    public class EmailTest
    {
        IConfiguration _iconfiguration;
        public EmailTest(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }
        [TestMethod]
        public void SendEmailTest()
        {
           
           
          //  var email = new Email();
          //   email.SendEmail("gorlingy@hotmail.com", "test", "test", "test");

        }
    }
}
