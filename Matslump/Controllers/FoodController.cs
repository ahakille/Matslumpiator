using Matslump.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using Matslump.Services;

namespace Matslump.Controllers
{
    public class FoodController : Controller
    {
        // GET: Food
        [HttpGet]
        public ActionResult All(int? page ,int? size)
        {
            int SizeofPage =20;
            if(size != null)
            {
                SizeofPage = size.Value ;
            }
            string search = Request.QueryString["search"];
            Receptmodels re = new Receptmodels();
            Foodservices food = new Foodservices();
            if (!string.IsNullOrEmpty(search))
            {
                re.Recept = food.GetFoodListForReceptView("SELECT recept.id_recept, recept.name, recept.description,recept.url_pic,recept.url_recept,recept.cookingtime,type_of_food.type_name,recept.average_rating,recept.occasion_id  FROM recept LEFT JOIN type_of_food ON recept.type_of_food_id = type_of_food.id WHERE name LIKE @search", Convert.ToInt32(User.Identity.Name), search);
                ViewBag.Myfood = re.GetFood("SELECT * FROM recept WHERE id_recept IN (SELECT recept_id FROM users_has_recept WHERE user_id =@id_user)", Convert.ToInt32(User.Identity.Name));
            }
            else
            {
                re.Recept = food.GetFoodListForReceptView("SELECT recept.id_recept, recept.name, recept.description,recept.url_pic,recept.url_recept,recept.cookingtime,type_of_food.type_name,recept.average_rating,recept.occasion_id  FROM recept LEFT JOIN type_of_food ON recept.type_of_food_id = type_of_food.id", Convert.ToInt32(User.Identity.Name),null);
                ViewBag.Myfood = re.GetFood("SELECT * FROM recept WHERE id_recept IN (SELECT recept_id FROM users_has_recept WHERE user_id =@id_user)", Convert.ToInt32(User.Identity.Name));
            }
            
            var recept = re.Recept;
            ViewBag.food = re.Recept;
            var pager = new Pager(re.Recept.Count, page, SizeofPage);

            var viewModel = new IndexViewModel
            {
                Items = recept.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager,
                Sökord = search,
                Size = SizeofPage
                
               
            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult All(IndexViewModel search)
        {
            var re = new Receptmodels();
            if (search == null)
            {
                return RedirectToAction("All");
            }
            if (search.Size == 0)
                search.Size = 20;
            Foodservices food = new Foodservices();
            re.Recept = food.GetFoodListForReceptView("SELECT recept.id_recept, recept.name, recept.description,recept.url_pic,recept.url_recept,recept.cookingtime,type_of_food.type_name,recept.average_rating,recept.occasion_id  FROM recept LEFT JOIN type_of_food ON recept.type_of_food_id = type_of_food.id WHERE name LIKE @search", Convert.ToInt32(User.Identity.Name),search.Sökord);
            ViewBag.Myfood = re.GetFood("SELECT * FROM recept WHERE id_recept IN (SELECT recept_id FROM users_has_recept WHERE user_id =@id_user)", Convert.ToInt32(User.Identity.Name));
            var recept = re.Recept;
            ViewBag.food = re.Recept;

            var pager = new Pager(re.Recept.Count, 1,search.Size);

            var viewModel = new IndexViewModel
            {
                Items = recept.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager,
                Sökord = search.Sökord,
                Size = search.Size
        };
            return View(viewModel);
            
        }
        [Authorize(Roles = "Admin")]
        public ActionResult AddNewFood()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult EditFood(int id,int page)
        {
            Receptmodels re = new Receptmodels();
            re.Recept = re.GetFood("SELECT * FROM recept WHERE id_recept =@id_user", id);
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
        [Authorize(Roles = "Admin")]
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