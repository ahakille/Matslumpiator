using Matslumpiator.Models;
using Matslumpiator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Matslumpiator.Controllers
{
    [Authorize]
    public class FoodController : Controller
    {
        private readonly IFoodServices _foodservice;
        public FoodController(IFoodServices foodServices)
        {
            _foodservice = foodServices;
        }
        [HttpGet]
        public ActionResult All(int? page ,int? size,string search , bool chicken,bool beef , bool fish , bool meat, bool pork, bool sausage , bool vego,string cookingtime)
        {
            int SizeofPage =20;
            if(size != null)
            {
                SizeofPage = size.Value ;
            }  
            if (string.IsNullOrEmpty(cookingtime))
            {
                cookingtime = "Över 60 minuter";
            } 
            
            var recept = new List<Receptmodels>();
          
            var sql = _foodservice.CreateSearchString(chicken,vego, fish,beef, pork,sausage,meat, false,search,cookingtime);
            recept = _foodservice.GetFoodListForReceptView("SELECT * FROM public.recept_search_view" + sql, Convert.ToInt32(User.Identity.Name), search);        
            ViewBag.Myfood = _foodservice.GetFood("SELECT * FROM recept WHERE id_recept IN (SELECT recept_id FROM users_has_recept WHERE user_id =@id_user)", Convert.ToInt32(User.Identity.Name));
            ViewBag.food = recept;
            var pager = new Pager(recept.Count, page, SizeofPage);

            var viewModel = new IndexViewModel
            {
                Items = recept.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager,
                Sökord = search,
                Size = SizeofPage,
                BeefFilter = beef,
                ChickenFilter = chicken,
                FishFilter = fish,
                MeatFilter = meat,
                PorkFilter = pork,
                SausageFilter = sausage,
                VegoFilter = vego,
                Cookingtime = cookingtime
                



            };

            var cooking = new List<string> { "Under 15 minuter", "Under 30 minuter", "Under 45 minuter", "Under 60 minuter", "Över 60 minuter" };
            ViewBag.Cookingtimes = cooking;
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult All(IndexViewModel model, int? size)
        {
            int SizeofPage = 20;
            if (size != null)
            {
                SizeofPage = size.Value;
            }


            var recept = new List<Receptmodels>();
            
            var sql = _foodservice.CreateSearchString(model.ChickenFilter , model.VegoFilter, model.FishFilter, model.BeefFilter, model.PorkFilter, model.SausageFilter, model.MeatFilter, false, model.Sökord,model.Cookingtime);

            recept = _foodservice.GetFoodListForReceptView("SELECT * FROM public.recept_search_view" + sql, Convert.ToInt32(User.Identity.Name),model.Sökord);


            ViewBag.Myfood = _foodservice.GetFood("SELECT * FROM recept WHERE id_recept IN (SELECT recept_id FROM users_has_recept WHERE user_id =@id_user)", Convert.ToInt32(User.Identity.Name));
           
            ViewBag.food = recept;

            var pager = new Pager(recept.Count, 1, SizeofPage);

            var viewModel = new IndexViewModel
            {
                Items = recept.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager,
                Sökord = model.Sökord,
                Size = SizeofPage,
                BeefFilter = model.BeefFilter,
                ChickenFilter = model.ChickenFilter,
                FishFilter = model.FishFilter,
                MeatFilter = model.MeatFilter,
                PorkFilter = model.PorkFilter,
                SausageFilter = model.SausageFilter,
                VegoFilter = model.VegoFilter,
                Cookingtime = model.Cookingtime
                
            };
            var cooking = new List<string> { "Under 15 minuter", "Under 30 minuter", "Under 45 minuter", "Under 60 minuter", "Över 60 minuter" };
            ViewBag.Cookingtimes = cooking;

            return View(viewModel);
            
        }
        public ActionResult Clear()
        {
            return RedirectToAction("ALL");
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
            _foodservice.AddNewFood(model.Name,model.Description,model.Url_pic,model.Url_recept,Convert.ToInt16(User.Identity.Name));
            return RedirectToAction("ALL", "Food");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult EditFood(int id,int page)
        {
            Receptmodels re = new Receptmodels();
            re.Recept = _foodservice.GetFood("SELECT * FROM recept WHERE id_recept =@id_user", id);
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
           
            _foodservice.EditFood(model.Id, model.Name, model.Description,model.Url_pic,model.Url_recept);
            return RedirectToAction("ALL", "Food",new { page = page });
        }

        public ActionResult AddToMyFood(int id ,int page, string search, bool chicken, bool beef, bool fish, bool meat, bool pork, bool sausage, bool vego, string cookingtime)
        {
            
            _foodservice.AddFoodToUser(User.Identity.Name, id);
            return RedirectToAction("ALL", "Food",new { page = page ,chicken = chicken ,beef=beef,fish = fish,meat = meat,pork = pork,sausage=sausage,vego=vego,cookingtime=cookingtime ,search});

        }
    }
}