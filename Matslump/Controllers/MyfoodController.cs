using Matslump.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Matslump.Controllers
{
    public class MyfoodController : Controller
    {
        // GET: Myfood
        public ActionResult Index()
        {
            List<Receptmodels> food_list = new List<Receptmodels>();
            Receptmodels re = new Receptmodels();
            re.recept = re.getFood("SELECT * FROM recept WHERE id_recept IN (SELECT recept_id FROM users_has_recept WHERE user_id =@id_user)", Convert.ToInt32( User.Identity.Name));
            ViewBag.food = food_list;
            return View(re);
        }

        // GET: Myfood/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

               

        // GET: Myfood/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // GET: Myfood/Delete/5
        public ActionResult Delete(int id)
        {
            Receptmodels re = new Receptmodels();
            re.removefood_user(User.Identity.Name, id);
            return RedirectToAction("index");
        }

        
    }
}
