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
        [RequireHttps]
        public ActionResult Index()
        {

            return View();
        }
        
        [AllowAnonymous]
        public ActionResult Notfound()
        {
            ViewBag.Message = "404 - Not Found.";

            return View();
        }
 
    }
}