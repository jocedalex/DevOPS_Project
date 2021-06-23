using DevOPS_Project.Data;
using DevOPS_Project.Models;
using DevOPS_Project.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevOPS_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserVM user)
        {
            if (ModelState.IsValid)
            {
                var confirm = await _context.ToolUser.Where(x => x.UserName == user.UserName).SingleOrDefaultAsync();
                if (confirm == null)
                {
                    TempData["success"] = "The user is not valid";
                    return RedirectToAction("Index");
                }
                else
                {

                    //var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                    //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, confirm.UserID.ToString()));
                    //identity.AddClaim(new Claim(ClaimTypes.Name, confirm.UserName));
                    //var principal = new ClaimsPrincipal(identity);

                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    //    new AuthenticationProperties { ExpiresUtc = DateTime.Now.AddDays(1), IsPersistent = true });

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, confirm.UserID.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Name, confirm.UserName));
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                        new AuthenticationProperties { ExpiresUtc = DateTime.Now.AddDays(2), IsPersistent = true });

                    return RedirectToAction("Index");
                }
            } 
            else
            {
                TempData["success"] = "The user is not valid";
                return RedirectToAction("Index");
            }
            

          
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
