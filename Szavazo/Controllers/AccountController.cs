

using Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Szavazo.Controllers;
using Szavazo.Models;

namespace Library.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly SzavazoService _service;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, SzavazoService service)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _service = service;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountViewModel model, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user ==null)
                {
                    ModelState.AddModelError("", " Helytelen E-mail cím vagy jelszó!");
                }
                else
                {

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index","Polls");
                    }
                        else
                        {

                            ModelState.AddModelError("", "Bejelentkezés sikertelen!");
                        }
                }
                
              
                
            }
            else
            {
                ModelState.AddModelError("", "Invalid Modelstate");
            }
            return View(model);
            //return RedirectToAction(nameof(HomeController.Index));
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new User { Email = model.Email, UserName=model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _service.OnNewUser(user);
                    //await _userManager.AddToRoleAsync(user, "User");
                    //await _signInManager.SignInAsync(user, false);
                    //return RedirectToLocal("Account/Login");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", result.Errors.First().Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(this.Login));
        }

        private IActionResult RedirectToLocal(String returnUrl)
        {
            //return Redirect("Home/Index.cshtml");
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(AccountController.Login));
            }
        }
    }
}
