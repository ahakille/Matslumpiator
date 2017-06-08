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
            Email.SendEmail("gorlingy@hotmail.com", "nicklas", "Testmail", "Hejhej");
            int user_id = Convert.ToInt32(User.Identity.Name);
            slump slump = new slump();
            DateTime date;
            date = DateTime.Now;
            date = slump.datefixer(date);
            slump.recepts =slump.Oldslumps(user_id, date.AddDays(-30) ,date.AddDays(-1));
            slump.list = slump.Weeknumbers(slump.recepts);
            ViewBag.thisweek = slump.Oldslumps(user_id, date.AddDays(-1), date.AddDays(5));
  

            return View(slump);
        }

        // GET: Slumpiator/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Slumpiator/Create
        public ActionResult Create(DateTime date)
        {
            slump slump = new slump();
            Receptmodels re = new Receptmodels();
            re.recept = slump.Slumplist(Convert.ToInt32(User.Identity.Name),date);
            ViewBag.check = re.recept[0].id;
            TempData["receptlista"] = re.recept;
            
            
            return View(re);
        }

        // POST: Slumpiator/Create
        [HttpPost]
        public ActionResult Create(Receptmodels model)
        {
            int user = Convert.ToInt16(User.Identity.Name);
            model.recept = (List<Receptmodels>)TempData["receptlista"];
            slump checkslump = new slump();
            bool check= checkslump.checkslump(model.recept[0].date, user);
            
            
            foreach (var item in model.recept)
            {
                slump slump = new slump();
                slump.SaveSlump(item.id, user, item.date,check);
            }

            return RedirectToAction("Index");
   
        }

        

       
    }
}
