using Microsoft.VisualStudio.TestTools.UnitTesting;
using Matslump.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matslump.Services;

namespace Matslump.Models.Tests
{
    [TestClass()]
    public class EmailTests
    {
        [TestMethod()]
        public void SendEmailTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EmailslumplistTest()
        {
            var list = new List<Receptmodels>();
            var recept = new Receptmodels();
            recept.Name = "Laxröra";
            recept.Url_pic = "1480.jpg";
            recept.Date = DateTime.Now;
            list.Add(recept);

            var body =Email.Emailslumplist("nicklas", "Hej hej de funkar bra", list);

            Email.SendEmail("gorlingy@hotmail.com", "nicklas", "test", body);

            Assert.IsNotNull(body);
            
        }
    }
}