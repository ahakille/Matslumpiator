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
            return View();
        }
        public ActionResult NewUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewUser(Users model)
        {
            try
            {
                Accountmodels User = new Accountmodels();
                Tuple<byte[], byte[]> password = User.Generatepass(model.Password);
                postgres sql = new postgres();
                sql.SqlNonQuery("INSERT INTO login (salt, key ,username,roles_id) VALUES (@par2,@par3,@par1,'1')", postgres.list = new List<NpgsqlParameter>()
            {
                new NpgsqlParameter("@par1", model.User),
                new NpgsqlParameter("@par2", password.Item1),
                new NpgsqlParameter("@par3", password.Item2)
            });

                return RedirectToAction("index", "home");
            }
            catch
            {
                return View();
            }
        }
    }
}