using Matslump.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Matslump.Controllers
{
    public class SlumpiatorController : Controller
    {
        // GET: Slumpiator
       
        public ActionResult Index()
        {
            //Email.SendEmail("gorlingy@hotmail.com", "nicklas", "Testmail", "Hejhej");
            int user_id = Convert.ToInt32(User.Identity.Name);
            slump slumpe = new slump();
            DateTime date;
            date = DateTime.Now;
            date = slumpe.datefixer(date);
            slumpe.recepts =slumpe.Oldslumps(user_id, date.AddDays(-30) ,date.AddDays(-1));
            slumpe.list = slumpe.Weeknumbers(slumpe.recepts);
            ViewBag.thisweek = slumpe.Oldslumps(user_id, date.AddDays(-1), date.AddDays(6));
            ViewBag.date = slump.GetIso8601WeekOfYear(DateTime.Now);
            ViewBag.date1 = slump.GetIso8601WeekOfYear(DateTime.Now.AddDays(7));
            date = DateTime.Now;
            date = slumpe.datefixer(date);
            ViewBag.nextweek = slumpe.Oldslumps(user_id, date.AddDays(6), date.AddDays(14));


            return View(slumpe);
        }

        // GET: Slumpiator/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Slumpiator/Create
        public ActionResult Create(DateTime date)
        {
            slump slumpe = new slump();
            Receptmodels re = new Receptmodels();
            re.Recept = slumpe.Slumplist(Convert.ToInt32(User.Identity.Name),date);
            ViewBag.check = re.Recept[0].Id;
            TempData["receptlista"] = re.Recept;
            ViewBag.date = slump.GetIso8601WeekOfYear(date);
            
            
            return View(re);
        }

        // POST: Slumpiator/Create
        [HttpPost]
        public ActionResult Create(Receptmodels model)
        {
            int user = Convert.ToInt16(User.Identity.Name);
            model.Recept = (List<Receptmodels>)TempData["receptlista"];
            slump checkslump = new slump();
            bool check= checkslump.checkslump(model.Recept[0].Date, user);
            
            
            foreach (var item in model.Recept)
            {
                slump slump = new slump();
                slump.SaveSlump(item.Id, user, item.Date,check);
            }

            return RedirectToAction("Index");
   
        }

        

       
    }
}
