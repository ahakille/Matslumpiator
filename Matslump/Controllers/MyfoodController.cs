using Matslump.Models;
using Matslump.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Matslump.Controllers
{
    public class MyfoodController : Controller
    {
        // GET: Myfood
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
        public ActionResult Delete(int id, int page)
        {
            var re = new Foodservices();
            re.removefood_user(User.Identity.Name, id);
            return RedirectToAction("index",new { page = page });
        }

        
    }
}
