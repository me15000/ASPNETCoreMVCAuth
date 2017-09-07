using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace LeX.Controllers
{
    public class AccountController : Controller
    {
        [Authorize(Policy = "Permission", Roles = "admin,system")]
        public IActionResult Index()
        {
            return View();
        }





        [HttpGet("account/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Sid, "usn"));
            identity.AddClaim(new Claim(ClaimTypes.Name, "name"));
            identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));


            return View();
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        [HttpGet("denied")]
        public IActionResult Denied()
        {
            return View();
        }
    }
}