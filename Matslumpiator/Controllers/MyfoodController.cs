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

        public MyfoodController(IFoodServices foodServices)
        {
            _foodServices = foodServices;
        }
        public ActionResult Index(int? page, int? size)
        {
            int SizeofPage = 20;
            if (size != null)
            {
                SizeofPage = size.Value;
            }
            var food_list = new List<Receptmodels>();
            var re = new Foodservices();
            food_list = re.GetFood("SELECT * FROM recept WHERE id_recept IN (SELECT recept_id FROM users_has_recept WHERE user_id =@id_user)", Convert.ToInt32( User.Identity.Name));
            var pager = new Pager(food_list.Count, page, SizeofPage);

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

        
    }
}
