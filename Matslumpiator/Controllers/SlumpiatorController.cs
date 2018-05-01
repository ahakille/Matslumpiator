using Matslumpiator.Models;
using Matslumpiator.Services;
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
            int user_id = Convert.ToInt32(User.Identity.Name);
            var slumpe = new Slumpservices();
            var Slumpmodel = new Slump();
            DateTime date;
            date = DateTime.Now;
            date = slumpe.datefixer(date);
            Slumpmodel.Recepts =slumpe.Oldslumps(user_id, date.AddDays(-30) ,date.AddDays(-1));
            Slumpmodel.List = slumpe.Weeknumbers(Slumpmodel.Recepts);
            ViewBag.thisweek = slumpe.Oldslumps(user_id, date.AddDays(-1), date.AddDays(6));
            ViewBag.date = slumpe.GetIso8601WeekOfYear(DateTime.Now);
            ViewBag.date1 = slumpe.GetIso8601WeekOfYear(DateTime.Now.AddDays(7));
            date = DateTime.Now;
            date = slumpe.datefixer(date);
            ViewBag.nextweek = slumpe.Oldslumps(user_id, date.AddDays(6), date.AddDays(14));


            return View(Slumpmodel);
        }


        // GET: Slumpiator/Create
        public ActionResult Create(int id)
        {
            DateTime date = DateTime.Now;
            if (id == 7)
            {
                date = date.AddDays(7);
            }

            var Slump = new Slumpservices();
            Receptmodels re = new Receptmodels();
            re.Recept = Slump.Slumplist(Convert.ToInt32(User.Identity.Name),date);
            ViewBag.check = re.Recept[0].Id;
            ViewBag.date = Slump.GetIso8601WeekOfYear(date);
            
            
            return View(re);
        }

        // POST: Slumpiator/Create
        [HttpPost]
        public ActionResult Create(IFormCollection form)
        {
           int user = Convert.ToInt16(User.Identity.Name);

            Slumpservices checkslump = new Slumpservices();
           var datetime = Convert.ToDateTime(form["0date"]);
           datetime =datetime.Date;
           bool check= checkslump.Checkslump(datetime, user);
            for (int i=0; i <5; i++ )
            {
                var id = Convert.ToInt16(form[i + "id"]);
                var date = Convert.ToDateTime(form[i + "date"]);

                var slump = new Slumpservices();
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

        [AllowAnonymous]
        public ActionResult RunSlump(string code)
        {
            if(code == "asdfg")
            {
                Slumpcron.StartCron();
                return Ok();
            }
            return BadRequest("Wrong");
           
        }

        

       
    }
}
