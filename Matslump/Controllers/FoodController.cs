using Matslump.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using Matslump.Services;
using System.Collections.Generic;

namespace Matslump.Controllers
{
    public class FoodController : Controller
    {
        // GET: Food
        [HttpGet]
        public ActionResult All(int? page ,int? size )
        {
            int SizeofPage =20;
            string search = "" ,cookingtime = "Över 60 minuter";
            bool chicken = false, biff = false ,fish = false , meat = false,pork = false,sausager = false,vego = false;
            if(size != null)
            {
                SizeofPage = size.Value ;
            }          
     
            var post = (IndexViewModel)Session["SearchFilter"];
            var recept = new List<Receptmodels>();
            var food = new Foodservices();
            if (post != null) //!string.IsNullOrEmpty(search)
            {
                var sql = food.CreateSearchString(post.ChickenFilter, post.VegoFilter, post.FishFilter, post.BeefFilter, post.PorkFilter, post.SausageFilter, post.MeatFilter, post.OtherFilter, post.Sökord,post.Cookingtime);
                recept = food.GetFoodListForReceptView("SELECT * FROM public.recept_search_view"+sql, Convert.ToInt32(User.Identity.Name), post.Sökord);
                chicken = post.ChickenFilter;
                biff = post.BeefFilter;
                search = post.Sökord;
                fish = post.FishFilter;
                meat = post.MeatFilter;
                pork = post.PorkFilter;
                sausager = post.SausageFilter;
                vego = post.VegoFilter;
                cookingtime = post.Cookingtime;
               
            }
            else
            {
                recept = food.GetFoodListForReceptView("SELECT * FROM public.recept_search_view", Convert.ToInt32(User.Identity.Name),null);
               
            }
            ViewBag.Myfood = food.GetFood("SELECT * FROM recept WHERE id_recept IN (SELECT recept_id FROM users_has_recept WHERE user_id =@id_user)", Convert.ToInt32(User.Identity.Name));
            ViewBag.food = recept;
            var pager = new Pager(recept.Count, page, SizeofPage);

            var viewModel = new IndexViewModel
            {
                Items = recept.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager,
                Sökord = search,
                Size = SizeofPage,
                BeefFilter = biff,
                ChickenFilter = chicken,
                FishFilter = fish,
                MeatFilter = meat,
                PorkFilter = pork,
                SausageFilter = sausager,
                VegoFilter = vego,
                Cookingtime = cookingtime
                



            };
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult All(FormCollection form, int? size)
        {
            int SizeofPage = 20;
            if (size != null)
            {
                SizeofPage = size.Value;
            }
            var Sökord = Request.Form["Sökord"];
            var cookingtime = Request.Form["cookingtime"];
            var ChickenFilter = Convert.ToBoolean(Request.Form["ChickenFilter"].Split(',')[0]);
            var VegoFilter = Convert.ToBoolean(Request.Form["VegoFilter"].Split(',')[0]);
            var FishFilter = Convert.ToBoolean(Request.Form["FishFilter"].Split(',')[0]);
            var BeefFilter = Convert.ToBoolean(Request.Form["BeefFilter"].Split(',')[0]);
            var PorkFilter = Convert.ToBoolean(Request.Form["PorkFilter"].Split(',')[0]);
            var SausageFilter = Convert.ToBoolean(Request.Form["SausageFilter"].Split(',')[0]);
            var MeatFilter = Convert.ToBoolean(Request.Form["MeatFilter"].Split(',')[0]);



            var food = new Foodservices();
            var recept = new List<Receptmodels>();
            
           var sql =food.CreateSearchString(ChickenFilter, VegoFilter, FishFilter, BeefFilter, PorkFilter, SausageFilter, MeatFilter, false, Sökord,cookingtime);

           recept = food.GetFoodListForReceptView("SELECT * FROM public.recept_search_view" + sql, Convert.ToInt32(User.Identity.Name),Sökord);


            ViewBag.Myfood = food.GetFood("SELECT * FROM recept WHERE id_recept IN (SELECT recept_id FROM users_has_recept WHERE user_id =@id_user)", Convert.ToInt32(User.Identity.Name));
           
            ViewBag.food = recept;

            var pager = new Pager(recept.Count, 1, SizeofPage);

            var viewModel = new IndexViewModel
            {
                Items = recept.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager,
                Sökord = Sökord,
                Size = SizeofPage,
                BeefFilter = BeefFilter,
                ChickenFilter = ChickenFilter,
                FishFilter = FishFilter,
                MeatFilter = MeatFilter,
                PorkFilter = PorkFilter,
                SausageFilter = SausageFilter,
                VegoFilter = VegoFilter,
                Cookingtime = cookingtime
                
            };
           Session["SearchFilter"] = viewModel;
            
            return View(viewModel);
            
        }
        public ActionResult Clear()
        {
            Session.RemoveAll();
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
            Receptmodels re = new Receptmodels();
            re.addNewFood(model.Name,model.Description,model.Url_pic,model.Url_recept,Convert.ToInt16(User.Identity.Name));
            return RedirectToAction("ALL", "Food");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult EditFood(int id,int page)
        {
            Receptmodels re = new Receptmodels();
            var Foodlist = new Foodservices();
            re.Recept = Foodlist.GetFood("SELECT * FROM recept WHERE id_recept =@id_user", id);
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