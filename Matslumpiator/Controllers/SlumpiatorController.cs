using Matslumpiator.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;


namespace Matslumpiator.Controllers
{
    [Authorize]
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
        public ActionResult Create(int id)
        {
            DateTime date = DateTime.Now;
            if (id == 7)
            {
                date = date.AddDays(7);
            }
            slump slumpe = new slump();
            Receptmodels re = new Receptmodels();
            re.Recept = slumpe.Slumplist(Convert.ToInt32(User.Identity.Name),date);
            ViewBag.check = re.Recept[0].Id;
            ViewBag.date = slump.GetIso8601WeekOfYear(date);
            
            
            return View(re);
        }

        // POST: Slumpiator/Create
        [HttpPost]
        public ActionResult Create(IFormCollection form)
        {
           int user = Convert.ToInt16(User.Identity.Name);
            
           slump checkslump = new slump();
           var datetime = Convert.ToDateTime(form["0date"]);
           datetime =datetime.Date;
           bool check= checkslump.Checkslump(datetime, user);
            for (int i=0; i <5; i++ )
            {
                var id = Convert.ToInt16(form[i + "id"]);
                var date = Convert.ToDateTime(form[i + "date"]);

                slump slump = new slump();
                slump.SaveSlump(id, user, date,check);
            }
            return RedirectToAction("Index");
   
        }

        public ActionResult AddRandomList(int food1,int food2,int food3)
        {
            var re = new Receptmodels();
            re.addFood_user(User.Identity.Name,food1);
            re.addFood_user(User.Identity.Name, food2);
            re.addFood_user(User.Identity.Name, food3);
            return View();
        }

        

       
    }
}
