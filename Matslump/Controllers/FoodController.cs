using Matslump.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Matslump.Controllers
{
    public class FoodController : Controller
    {
        // GET: Food
        public ActionResult All()
        {
            List<Receptmodels> food_list = new List<Receptmodels>();
            Receptmodels re = new Receptmodels();
            re.recept = re.getFood("SELECT * FROM recept ",Convert.ToInt32(User.Identity.Name));
            ViewBag.Myfood= re.getFood("SELECT * FROM recept WHERE id_recept IN (SELECT recept_id FROM users_has_recept WHERE user_id =@id_user)", Convert.ToInt32(User.Identity.Name));

            ViewBag.food = re.recept;
            return View(re);
        }
        public ActionResult AddNewFood()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewFood(Receptmodels model)
        {
            Receptmodels re = new Receptmodels();
            re.addNewFood(model.name,model.description,Convert.ToInt16(User.Identity.Name));
            return RedirectToAction("ALL", "Food");
        }
        public ActionResult EditFood(int id)
        {
            Receptmodels re = new Receptmodels();
            re.recept = re.getFood("SELECT * FROM recept WHERE id_recept =@id_user", id);
            re.Id = re.recept[0].Id;
            re.name = re.recept[0].name;
            re.description = re.recept[0].description;

            return View(re);
        }
        [HttpPost]
        public ActionResult EditFood(Receptmodels model)
        {
            Receptmodels re = new Receptmodels();
            re.EditFood(model.Id, model.name, model.description);
            return RedirectToAction("ALL", "Food");
        }

        public ActionResult AddToMyFood(int id)
        {
            Receptmodels re = new Receptmodels();
            re.addFood_user(User.Identity.Name, id);
            return RedirectToAction("ALL", "Food");

        }
    }
}