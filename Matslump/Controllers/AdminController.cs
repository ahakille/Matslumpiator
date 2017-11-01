using Matslump.Models;
using Matslump.Services;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Matslump.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [HttpGet]
        [Authorize(Roles= "Admin")]
        public ActionResult Index()
        {
            Users us = new Users();
            //Behöver skrivas om! klar
            ViewBag.userlist= us.GetuserAsAdmin(0, "SELECT users.user_id,users.username, users.fname, users.last_name, users.email,users.acc_active,users.roles_id ,users.last_login ,users.settings_id  FROM public.users ORDER BY users.last_login DESC");
            
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
            Users us = new Users();
            List<Users> list = new List<Users>();
            // Behöver skrivas om! klar
            list = us.GetuserAsAdmin(id, "SELECT users.user_id,users.username, users.fname, users.last_name, users.email,users.acc_active,users.roles_id,last_login,users.settings_id FROM public.users WHERE user_id = @id");
            us.active = list[0].active;
            us.email = list[0].email;
            us.User = list[0].User;
            us.Roles_id = list[0].Roles_id;
            us.User_id = id;
            return View(us);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult NewUser(Users model)
        {
            try
            {
                Accountmodels User = new Accountmodels();
                Tuple<byte[], byte[]> password = User.Generatepass(model.Password);
                postgres sql = new postgres();
                // Behöver skrivas om
                sql.SqlNonQuery("INSERT INTO login (salt, key ,username,roles_id,email,acc_active,last_login) VALUES (@par2,@par3,@par1,'2',@email,@active,@last_login)", postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", model.User),
                new NpgsqlParameter("@par2", password.Item1),
                new NpgsqlParameter("@email", model.email),
                new NpgsqlParameter("@active", model.active),
                new NpgsqlParameter("@last_login", DateTime.Now),
                new NpgsqlParameter("@par3", password.Item2)
            });

                return RedirectToAction("index", "admin");
            }
            catch
            {
                return View();
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult SendMessage(FormCollection form)
        {
            string message =Request.Form["message"];
            string subject = Request.Form["subject"];
            Users us = new Users();
            // Behöver skrivas om! klar
            List<Users> list = us.GetuserAsAdmin(0, "SELECT users.user_id,users.username, users.fname, users.last_name, users.email,users.acc_active,users.roles_id ,users.last_login,users.day_of_slumpcron FROM public.users");
            foreach (var item in list)
            {
                Email.SendEmail(item.email, item.User, subject,Email.EmailOther(subject,message));
                
            }
            return RedirectToAction("index", "admin");
        }
    }
}