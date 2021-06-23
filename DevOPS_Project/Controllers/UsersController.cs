using DevOPS_Project.Data;
using DevOPS_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevOPS_Project.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {

        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Buildings Index GET
        public IActionResult Index()
        {
            IEnumerable<ToolUser> listUser = _context.ToolUser;
            return View(listUser);
        }


        //Create new entries GET
        public IActionResult Create()
        {

            return View();
        }

        //Create new entries POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToolUser user)
        {

            if(ModelState.IsValid)
            {

                var confirm = await _context.ToolUser.Where(x => x.UserName == user.UserName).SingleOrDefaultAsync();
                if (confirm != null)
                {
                    TempData["success"] = "The user already exist";
                    return RedirectToAction("Create");
                }
                else {
                    user.SyscreatedDt = DateTime.Now;
                    _context.ToolUser.Add(user);
                    _context.SaveChanges();

                    TempData["success"] = "The new user has been created";
                    return RedirectToAction("Index");
                }
                
            }

            return View();
        }

        

        //Delete field GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //Get Building
            var user = _context.ToolUser.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        
        //Delete Field POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(ToolUser user)
        {

            
            if (user == null)
            {
                return NotFound();
            }

              
            _context.ToolUser.Remove(user);
            _context.SaveChanges();

            TempData["success"] = "The user has been removed";
            return RedirectToAction("Index");
            

        }
    }
}
