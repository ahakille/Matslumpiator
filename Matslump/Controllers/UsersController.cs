using Matslump.Models;
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
            Users us = new Users();
            List<Users> list = new List<Users>();
            list = us.Getuser(id, "SELECT users.user_id,users.username,users.email,users.acc_active,users.roles_id,last_login,users.settings_id FROM public.users WHERE user_id = @id");
            us.active = list[0].active;
            us.email = list[0].email;
            us.User = list[0].User;
            us.Roles_id = list[0].Roles_id;
            us.Last_login = list[0].Last_login;
            ViewBag.weeklist = Weeklist.List(); 
            us.User_id = id;
            return View(us);
           
        }

        public ActionResult Edit()
        {
            int id = Convert.ToInt32(User.Identity.Name);
            Users us = new Users();
            List<Users> list = new List<Users>();
            list = us.Getuser(id, "SELECT users.user_id,users.username,users.email,users.acc_active,users.roles_id,users.last_login,usersettings.day_of_slumpcron FROM public.users LEFT JOIN usersettings ON setting_id = users.user_id WHERE user_id = @id");
            us.active = list[0].active;
            us.email = list[0].email;
            us.User = list[0].User;
            us.Roles_id = list[0].Roles_id;
            ViewBag.weeklist = Weeklist.List();
            us.User_id = id;
            return View(us);
        }
        [HttpPost]
        public ActionResult Edit(Users model)
        {
            Users us = new Users();
            us.UpdateUser(model.User_id, model.User, model.email, model.active);
            return RedirectToAction("index");
        }
    }
}