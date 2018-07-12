using Matslumpiator.Models;
using Matslumpiator.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Matslumpiator.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService options)
        {
            _accountService = options;

        }
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
        public async Task<IActionResult> Index(Accountmodels model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var result = _accountService.AuthenticationUser(model.Password, model.User);
            if (result.Item2 == true)
            {

                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name,Convert.ToString(result.Item1)),
                    new Claim(ClaimTypes.Role,result.Item3),
                    new Claim(ClaimTypes.GivenName,result.Item3)
                };
                var authProperties = new AuthenticationProperties { };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity),authProperties);
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                {
                      return Redirect(model.ReturnUrl);
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
            if (!ModelState.IsValid)
            {
                // om inte rätt format
                return View(model);

            }

            postgres sql = new postgres();
            bool check = sql.SqlQueryExist("Select exists(SELECT users.username FROM public.users WHERE users.username = @par1);", postgres.list = new List<NpgsqlParameter>() { new NpgsqlParameter("@par1", model.User) });
            if (!check)
            {
               
                _accountService.RegisterNewUser(model.User, model.email, model.First_name, model.Last_name);
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
        public ActionResult Forgetpassword(Accountmodels model)
        {
          

            bool result = _accountService.Forgetpassword(model.User);
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
            UserService us = new UserService();
            
            Tuple<int, bool, string> reset = _accountService.Resetpassword(validate);
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
        public ActionResult Resetpassword(UserService model)
        {
            _accountService.Newpassword(model.Login_id, model.Password);
            return RedirectToAction("Index", "Account");
        }

        public async Task<IActionResult> LogOff()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [RequireHttps]
        public ActionResult Newpassword()
        {
            return View();
        }
        [RequireHttps]
        [HttpPost]
        public ActionResult Newpassword(UserService model)
        {

            try
            {
                postgres sql = new postgres();
                int id = sql.SqlQueryString("SELECT login_id FROM users WHERE user_id = @id", postgres.list = new List<NpgsqlParameter>()
                     {  new NpgsqlParameter("@id",Convert.ToInt16(User.Identity.Name)) });

                _accountService.Newpassword(id, model.Password);

                return RedirectToAction("index", "users");
            }
            catch
            {
                return View();
            }
        }
    }

}