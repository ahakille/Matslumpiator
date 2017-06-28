using Matslump.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Matslump.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [AllowAnonymous]
        //[RequireHttps]
        public ActionResult Index(string returnUrl)
        {
            
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        //[RequireHttps]
        public ActionResult Index(Accountmodels model)
        {
            if (!ModelState.IsValid)
            {
                // om inte rätt format
                return View(model);
            
            }
            Accountmodels acc = new Accountmodels();
            var result  = acc.AuthenticationUser(model.Password, model.user);
            if (result.Item2 == true)
            {
               
                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name,Convert.ToString(result.Item1)),
                    new Claim(ClaimTypes.Role,result.Item3),
                    new Claim(ClaimTypes.GivenName,result.Item3)},"ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignIn(identity);
               
                return Redirect("home/index");
            }
            else
            {
                ModelState.AddModelError("", "Fel lösenord eller användanamn");
                return View(model);
            }


        }
        [AllowAnonymous]
        //[RequireHttps]
        public ActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        //[RequireHttps]
        public ActionResult Register(Users model)
        {
            if (model.Secret == "Nicklas" || model.Secret == "nicklas")
            {
                postgres sql = new postgres();
                bool check = sql.SqlQueryExist("Select exists(SELECT users.username FROM public.users WHERE users.username = @par1);", postgres.list = new List<NpgsqlParameter>() { new NpgsqlParameter("@par1", model.User) });
                if (!check)
                {
                    Accountmodels User = new Accountmodels();
                    User.RegisterNewUser(model.User, model.email);
                    return RedirectToAction("Index", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Användarnamnet finns redan, Välj ett annat");
                    return View(model);
                }
                
            }
            else
            {
                ModelState.AddModelError("", "Fel Registeringskod");
                return View(model);
            }
           
           
        }
        //[RequireHttps]
        [AllowAnonymous]
        public ActionResult Forgetpassword()
        {

            return View();
        }
        [HttpPost]
        //[RequireHttps]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Forgetpassword(Users model)
        {
            Accountmodels acc = new Accountmodels();

            bool result = acc.Forgetpassword(model.User);
            if(result)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Något gick fel");
            return View(model);
        }
        public ActionResult Resetpassword(string validate)
        {
            if(string.IsNullOrEmpty(validate))
            {
                return RedirectToAction("Index", "Home");
            }
            

            return View()
        }

        public ActionResult LogOff()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }
    }
    
}