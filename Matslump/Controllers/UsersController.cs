﻿using Matslump.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Matslump.Tools;
using Matslump.Services;

namespace Matslump.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            int id = Convert.ToInt32(User.Identity.Name);
            UsersEditViewmodel us = new UsersEditViewmodel();
            Users us1 = new Users();
            List<UsersEditViewmodel> list = new List<UsersEditViewmodel>();
            list = us1.Getuser(id, "SELECT users.user_id,users.username,users.email,users.last_name, users.fname, usersettings.day_of_slumpcron FROM public.users LEFT JOIN usersettings ON setting_id = users.user_id WHERE user_id =@id");
            us.First_name = list[0].First_name;
            us.email = list[0].email;
            us.User = list[0].User;
            us.Last_name = list[0].Last_name;
            us.CronoDay = list[0].CronoDay;
            ViewBag.weeklist = Weeklist.List(); 
            us.User_id = id;
            return View(us);
           
        }
        public ActionResult Edit()
        {
            int id = Convert.ToInt32(User.Identity.Name);
            UsersEditViewmodel us = new UsersEditViewmodel();
            Users us1 = new Users();
            List<UsersEditViewmodel> list = new List<UsersEditViewmodel>();
            list = us1.Getuser(id, "SELECT users.user_id,users.username,users.email,users.last_name, users.fname, usersettings.day_of_slumpcron FROM public.users LEFT JOIN usersettings ON setting_id = users.settings_id WHERE user_id =@id");
            us.First_name = list[0].First_name;
            us.email = list[0].email;
            us.User = list[0].User;
            us.Last_name = list[0].Last_name;
            us.CronoDay = list[0].CronoDay;
            ViewBag.weeklist = Weeklist.List();
            us.User_id = id;
            return View(us);
        }
        [HttpPost]
        public ActionResult Edit(UsersEditViewmodel model)
        {
            Users us = new Users();
            us.UpdateUser(model.User_id, model.User, model.email, model.First_name,model.Last_name,model.CronoDay);
            return RedirectToAction("Edit");
        }
    }
}