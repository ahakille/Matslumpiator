using Matslump.Models;
using Matslump.Services;
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
        [RequireHttps]
        public ActionResult Index(string returnUrl)
        {

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireHttps]
        public ActionResult Index(Accountmodels model)
        {
            if (!ModelState.IsValid)
            {
                // om inte rätt format
                return View(model);

            }
            Accountservice acc = new Accountservice();
            var result = acc.AuthenticationUser(model.Password, model.user);
            if (result.Item2 == true)
            {

                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name,Convert.ToString(result.Item1)),
                    new Claim(ClaimTypes.Role,result.Item3),
                    new Claim(ClaimTypes.GivenName,result.Item3)}, "ApplicationCookie");

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignIn(identity);
                if (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                {
                    return Redirect(Request.QueryString["ReturnUrl"]);
                }

                return Redirect("home/index");
            }
            else
            {
                ModelState.AddModelError("", "Fel lösenord eller användanamn");
                return View(model);
            }


        }
        [AllowAnonymous]
        [RequireHttps]
        public ActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [RequireHttps]
        public ActionResult Register(CreateAccountViewmodel model)
        {


            postgres sql = new postgres();
            bool check = sql.SqlQueryExist("Select exists(SELECT users.username FROM public.users WHERE users.username = @par1);", postgres.list = new List<NpgsqlParameter>() { new NpgsqlParameter("@par1", model.User) });
            if (!check)
            {
                Accountservice User = new Accountservice();
                User.RegisterNewUser(model.User, model.email, model.First_name, model.Last_name);
                return RedirectToAction("Index", "Account");
            }
            else
            {
                ModelState.AddModelError("", "Användarnamnet finns redan, Välj ett annat");
                return View(model);
            }





        }
        [RequireHttps]
        [AllowAnonymous]
        public ActionResult Forgetpassword(int id)
        {
            if (id == 1)
            {
                ModelState.AddModelError("", "Din tid för byte har gått ut eller felaktigt återställningskod. Vänligen skapa en ny");
                return View();
            }
            return View();
        }
        [HttpPost]
        [RequireHttps]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Forgetpassword(Users model)
        {
            Accountservice acc = new Accountservice();

            bool result = acc.Forgetpassword(model.User);
            if (result)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Användaren finns inte eller så är kontot inte aktiverat");
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult Resetpassword(string validate)
        {
            if (string.IsNullOrEmpty(validate))
            {
                return RedirectToAction("Index", "Home");
            }
            Users us = new Users();
            Accountservice acc = new Accountservice();
            Tuple<int, bool, string> reset = acc.Resetpassword(validate);
            if (reset.Item2)
            {
                us.Login_id = reset.Item1;
                us.User = reset.Item3;
                return View(us);
            }
            return RedirectToAction("Forgetpassword", "Account", 1);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Resetpassword(Users model)
        {
            Users us = new Users();
            us.Newpassword(model.Login_id, model.Password);
            return RedirectToAction("Index", "Account");
        }

        public ActionResult LogOff()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }
        [RequireHttps]
        public ActionResult Newpassword()
        {
            return View();
        }
        [RequireHttps]
        [HttpPost]
        public ActionResult Newpassword(Users model)
        {

            try
            {
                postgres sql = new postgres();
                int id = sql.SqlQueryString("SELECT login_id FROM users WHERE user_id = @id", postgres.list = new List<NpgsqlParameter>()
                     {  new NpgsqlParameter("@id",Convert.ToInt16(User.Identity.Name)) });
                Users us = new Users();
                us.Newpassword(id, model.Password);

                return RedirectToAction("index", "users");
            }
            catch
            {
                return View();
            }
        }
    }

}