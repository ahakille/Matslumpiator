using Matslumpiator.Models;
using Matslumpiator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Matslumpiator.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IUserServices _userServices;

        public AdminController(IUserServices userServices)
        {
            _userServices = userServices;

        }
        // GET: Admin
        [HttpGet]
        [Authorize(Roles= "Admin")]
        public ActionResult Index()
        {
            ViewBag.userlist= _userServices.GetuserAsAdmin(0, "SELECT users.user_id,users.username, users.fname, users.last_name, users.email,users.acc_active,users.roles_id ,users.last_login ,users.settings_id  FROM public.users ORDER BY users.last_login DESC");
            
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult NewUser()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            
            List<UserService> list = new List<UserService>();
            UserService user = new UserService();
            // Behöver skrivas om! klar
            list = _userServices.GetuserAsAdmin(id, "SELECT users.user_id,users.username, users.fname, users.last_name, users.email,users.acc_active,users.roles_id,last_login,users.settings_id FROM public.users WHERE user_id = @id");
            user.active = list[0].active;
            user.email = list[0].email;
            user.User = list[0].User;
            user.Roles_id = list[0].Roles_id;
            user.User_id = id;
            return View(user);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult NewUser(UserService model)
        {
            try
            {
                

                return RedirectToAction("index", "admin");
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SendMessage(IFormCollection form)
        {
            string message =form["message"];
            string subject = form["subject"];

            await _userServices.SendMessagesToAllUsers(subject, message);

            return RedirectToAction("index", "admin");
        }
    }
}