using Matslumpiator.Models;
using Matslumpiator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;


namespace Matslumpiator.Controllers
{
    [Authorize]
    public class SlumpiatorController : Controller
    {
        private readonly ISlumpCronService _slumpCronService;
        private readonly ISlumpServices _slumpServices;
        private readonly IFoodServices _foodServices;

        public SlumpiatorController(ISlumpCronService slumpCronService,ISlumpServices slumpServices, IFoodServices foodServices)
        {
            _slumpCronService = slumpCronService;
            _slumpServices = slumpServices;
            _foodServices = foodServices;

        }

        public ActionResult Index()
        {
            int user_id = Convert.ToInt32(User.Identity.Name);
            var Slumpmodel = new Slump();
            DateTime date;
            date = DateTime.Now;
            date = _slumpServices.datefixer(date);
            Slumpmodel.Recepts = _slumpServices.Oldslumps(user_id, date.AddDays(-30) ,date.AddDays(-1));
            Slumpmodel.List = _slumpServices.Weeknumbers(Slumpmodel.Recepts);
            ViewBag.thisweek = _slumpServices.Oldslumps(user_id, date.AddDays(-1), date.AddDays(6));
            ViewBag.date = _slumpServices.GetIso8601WeekOfYear(DateTime.Now);
            ViewBag.date1 = _slumpServices.GetIso8601WeekOfYear(DateTime.Now.AddDays(7));
            date = DateTime.Now;
            date = _slumpServices.datefixer(date);
            ViewBag.nextweek = _slumpServices.Oldslumps(user_id, date.AddDays(6), date.AddDays(14));


            return View(Slumpmodel);
        }

        public ActionResult Create(int id)
        {
            DateTime date = DateTime.Now;
            if (id == 7)
            {
                date = date.AddDays(7);
            }

            Receptmodels re = new Receptmodels();
            re.Recept = _slumpServices.Slumplist(Convert.ToInt32(User.Identity.Name),date);
            ViewBag.check = re.Recept[0].Id;
            ViewBag.date = _slumpServices.GetIso8601WeekOfYear(date);
            
            
            return View(re);
        }

        [HttpPost]
        public ActionResult Create(IFormCollection form)
        {
           int user = Convert.ToInt16(User.Identity.Name);

           var datetime = Convert.ToDateTime(form["0date"]);
           datetime =datetime.Date;
           bool check= _slumpServices.Checkslump(datetime, user);
            for (int i=0; i <5; i++ )
            {
                var id = Convert.ToInt16(form[i + "id"]);
                var date = Convert.ToDateTime(form[i + "date"]);
                _slumpServices.SaveSlump(id, user, date,check);
            }
            return RedirectToAction("Index");
   
        }

        public ActionResult AddRandomList(int food1,int food2,int food3)
        {
            var re = new Receptmodels();
            _foodServices.AddFoodToUser(User.Identity.Name,food1);
            _foodServices.AddFoodToUser(User.Identity.Name, food2);
            _foodServices.AddFoodToUser(User.Identity.Name, food3);
            return View();
        }

        [AllowAnonymous]
        public ActionResult RunSlump(string code)
        {
            if(code == "asdfg")
            {
                _slumpCronService.StartCron();
                return Ok();
            }
            return BadRequest("Wrong");
           
        }
       
    }
}
