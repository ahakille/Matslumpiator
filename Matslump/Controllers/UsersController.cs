using Matslump.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

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
            list = us.Getuser(id, "SELECT login.user_id,login.username,login.email,login.acc_active,login.roles_id,last_login FROM public.login WHERE user_id = @id");
            us.active = list[0].active;
            us.email = list[0].email;
            us.User = list[0].User;
            us.Roles_id = list[0].Roles_id;
            us.Last_login = list[0].Last_login;
            us.User_id = id;
            return View(us);
           
        }
        public ActionResult Newpassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Newpassword(Users model)
        {

            try
            {
                Accountmodels User1 = new Accountmodels();
                Tuple<byte[], byte[]> password = User1.Generatepass(model.Password);
                postgres sql = new postgres();
                sql.SqlNonQuery("UPDATE login set salt= @par2, key =@par3 WHERE user_id =@par1", postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", Convert.ToInt16(User.Identity.Name)),
                new NpgsqlParameter("@par2", password.Item1),
                new NpgsqlParameter("@par3", password.Item2)
            });

                return RedirectToAction("index", "users");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit()
        {
            int id = Convert.ToInt32(User.Identity.Name);
            Users us = new Users();
            List<Users> list = new List<Users>();
            list = us.Getuser(id, "SELECT login.user_id,login.username,login.email,login.acc_active,login.roles_id,last_login FROM public.login WHERE user_id = @id");
            us.active = list[0].active;
            us.email = list[0].email;
            us.User = list[0].User;
            us.Roles_id = list[0].Roles_id;
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