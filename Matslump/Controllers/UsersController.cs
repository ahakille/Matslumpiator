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
            return View();
        }
        public ActionResult Newpassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Newpassword(Users model)
        {
            //    if (!ModelState.IsValid)
            //    {
            //        // om inte rätt format
            //        return View(model);
            //    }
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
    }
}