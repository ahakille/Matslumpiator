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
    public class MyfoodController : Controller
    {
        private readonly IFoodServices _foodServices;
        private readonly ISlumpServices _slumpServices;

        public MyfoodController(IFoodServices foodServices, ISlumpServices slumpServices)
        {
            _foodServices = foodServices;
            _slumpServices = slumpServices;
        }
        public ActionResult Index(int? page, int? size)
        {
            int SizeofPage = 20;
            if (size != null)
            {
                SizeofPage = size.Value;
            }
            var food_list = new List<Receptmodels>();
            food_list = _foodServices.GetFood("SELECT * FROM recept WHERE id_recept IN (SELECT recept_id FROM users_has_recept WHERE user_id =@id_user)", Convert.ToInt32( User.Identity.Name));
            var pager = new Pager(food_list.Count, page, SizeofPage);
            ViewBag.Count = food_list.Count;
            var viewModel = new IndexViewModel
            {
                Items = food_list.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager,
                Size = SizeofPage
            };
            return View(viewModel);
            
        }

        // GET: Myfood/Delete/5
        public ActionResult Delete(int id, int page)
        {

            _foodServices.RemovefoodFromUser(User.Identity.Name, id);
            return RedirectToAction("index",new { page = page });
        }
        
        public ActionResult NewSuggestions()
        {
            ViewBag.recepts =  _slumpServices.CreateRandomListOfRecept();

            return View();
        }
        public ActionResult AddNewSuggestions(int id)
        {
            _foodServices.AddFoodToUser(User.Identity.Name, id);
            return RedirectToAction("NewSuggestions");
        }



    }
}
