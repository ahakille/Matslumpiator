using Matslump.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Matslump.Controllers
{
    public class MyfoodController : Controller
    {
        // GET: Myfood
        public ActionResult Index(int? page)
        {
            List<Receptmodels> food_list = new List<Receptmodels>();
            Receptmodels re = new Receptmodels();
            re.Recept = re.getFood("SELECT * FROM recept WHERE id_recept IN (SELECT recept_id FROM users_has_recept WHERE user_id =@id_user)", Convert.ToInt32( User.Identity.Name));
            var recept = re.Recept;
            var pager = new Pager(re.Recept.Count, page);

            var viewModel = new IndexViewModel
            {
                Items = recept.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
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
            Receptmodels re = new Receptmodels();
            re.removefood_user(User.Identity.Name, id);
            return RedirectToAction("index",new { page = page });
        }

        
    }
}
