using Matslump.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Matslump.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            Users us = new Users();

            ViewBag.userlist= us.Getuser(0, "SELECT login.user_id,login.username,login.email,login.acc_active,login.roles_id ,login.last_login FROM public.login");
            
            return View();
        }
        public ActionResult NewUser()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            Users us = new Users();
            List<Users> list = new List<Users>();
            list = us.Getuser(id, "SELECT login.user_id,login.username,login.email,login.acc_active,login.roles_id FROM public.login WHERE user_id = @id");
            us.active = list[0].active;
            us.email = list[0].email;
            us.User = list[0].User;
            us.Roles_id = list[0].Roles_id;
            us.User_id = id;
            return View(us);
        }
        [HttpPost]
        public ActionResult NewUser(Users model)
        {
            try
            {
                Accountmodels User = new Accountmodels();
                Tuple<byte[], byte[]> password = User.Generatepass(model.Password);
                postgres sql = new postgres();
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
    }
}