using Matslumpiator.Models;
using Matslumpiator.Services;
using Matslumpiator.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Matslumpiator.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserServices _userServices;

        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;

        }
        // GET: Users
        public ActionResult Edit()
        {
            int id = Convert.ToInt32(User.Identity.Name);
            UsersEditViewmodel us = new UsersEditViewmodel();
            List<UsersEditViewmodel> list = new List<UsersEditViewmodel>();
            list = _userServices.Getuser(id, "SELECT users.user_id,users.username,users.email,users.last_name, users.fname, usersettings.day_of_slumpcron FROM public.users LEFT JOIN usersettings ON setting_id = users.settings_id WHERE user_id =@id");
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
            int numberofDay= Weeklist.CheckCronoDay(model.CronoDay);
            UserService us = new UserService();
            us.UpdateUser(model.User_id, model.User, model.email, model.First_name,model.Last_name,numberofDay);
            return RedirectToAction("Edit");
        }
        public ActionResult ForgetMe()
        {
            int id = Convert.ToInt32(User.Identity.Name);
            _userServices.DeleteUser(id);
            return RedirectToActionPermanent("LogOff", "Account", null);
        }

    }
}