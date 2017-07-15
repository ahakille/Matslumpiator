using Matslump.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Matslump.Controllers
{
    public class FoodController : Controller
    {
        // GET: Food
        public ActionResult All(int? page)
        {
            Receptmodels re = new Receptmodels();
            re.Recept = re.getFood("SELECT * FROM recept ",Convert.ToInt32(User.Identity.Name));
            ViewBag.Myfood= re.getFood("SELECT * FROM recept WHERE id_recept IN (SELECT recept_id FROM users_has_recept WHERE user_id =@id_user)", Convert.ToInt32(User.Identity.Name));
            var recept = re.Recept;
            ViewBag.food = re.Recept;
            var pager = new Pager(re.Recept.Count, page);

            var viewModel = new IndexViewModel
            {
                Items = recept.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };
            return View(viewModel);
        }
        public ActionResult AddNewFood()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewFood(Receptmodels model)
        {
            if(string.IsNullOrWhiteSpace(model.Url_pic))
            {
                model.Url_pic = "http://www.ica.se/imagevaultfiles/id_53491/cf_6901/ica_recept.png";
            }
            if(string.IsNullOrWhiteSpace(model.Url_recept))
            {
                model.Url_recept = "#";
            }
            Receptmodels re = new Receptmodels();
            re.addNewFood(model.Name,model.Description,model.Url_pic,model.Url_recept,Convert.ToInt16(User.Identity.Name));
            return RedirectToAction("ALL", "Food");
        }
        public ActionResult EditFood(int id,int page)
        {
            Receptmodels re = new Receptmodels();
            re.Recept = re.getFood("SELECT * FROM recept WHERE id_recept =@id_user", id);
            re.Id = re.Recept[0].Id;
            re.Name = re.Recept[0].Name;
            re.Url_pic = re.Recept[0].Url_pic;
            re.Url_recept = re.Recept[0].Url_recept;
            re.Description = re.Recept[0].Description;
            ViewBag.page = page;
            TempData["page"] = page;
            return View(re);
        }
        [HttpPost]
        public ActionResult EditFood(Receptmodels model)
        {
            if (string.IsNullOrWhiteSpace(model.Url_pic))
            {
                model.Url_pic = "http://www.ica.se/imagevaultfiles/id_53491/cf_6901/ica_recept.png";
            }
            if (string.IsNullOrWhiteSpace(model.Url_recept))
            {
                model.Url_recept = "#";
            }
            int page = (int)TempData["page"];
            Receptmodels re = new Receptmodels();
            re.EditFood(model.Id, model.Name, model.Description,model.Url_pic,model.Url_recept);
            return RedirectToAction("ALL", "Food",new { page = page });
        }

        public ActionResult AddToMyFood(int id ,int page)
        {
            Receptmodels re = new Receptmodels();
            re.addFood_user(User.Identity.Name, id);
            return RedirectToAction("ALL", "Food",new { page = page });

        }
    }
}