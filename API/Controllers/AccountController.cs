
using Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Persistence.DTO;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;

        public AccountController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }
        //api/Account/login
        //
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(login.Email);
            var result = await _signInManager.PasswordSignInAsync(user.UserName, login.Password, false, false);
            if (result.Succeeded)
            {
                return Ok();
            }
            return Unauthorized("Login failed!");
        }
        //api/Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
