using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Matslump.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        //[RequireHttps]
        public ActionResult Index()
        {

            return View();
        }
        [RequireHttps]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [RequireHttps]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}