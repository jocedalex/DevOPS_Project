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
    public class BuildingsController : Controller
    {

        private readonly ApplicationDbContext _context;

        public BuildingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Buildings Index GET
        public IActionResult Index()
        {
            IEnumerable<Building> listBuilding = _context.Building;
            return View(listBuilding);
        }

        [HttpGet]
        public async Task<IActionResult> GetData()//Get data for table
        {
            var all = await _context.Building.Select(i=> new
            {
                buildingID = i.BuildingID,
                buildingName = i.BuildingName,
                syscreatedDt = i.SyscreatedDt.ToString("dd-MM-yyyy hh:mm:ss tt"),
                actions = "<a href='Buildings/Edit/"+i.BuildingID+ "' class='btn btn-warning' style='padding-right:10px'>Update</a> " +
                "<a href='Buildings/Delete/" + i.BuildingID+ "' class='btn btn-danger'>Delete</a> " +
                "<a href='Rooms/Index/" + i.BuildingID+"' class='btn btn-info'>Rooms</a>",
            }).ToListAsync();

            return Json(new { data = all });
        }


        //Create new entries GET
        public IActionResult Create()
        {

            return View();
        }

        //Create new entries POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Building building)
        {

            if(ModelState.IsValid)
            {
                building.SyscreatedDt = DateTime.Now;
                _context.Building.Add(building);
                _context.SaveChanges();

                TempData["success"] = "The new building has been created";
                return RedirectToAction("Index");
            }

            return View();
        }

        //Edit field GET
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            //Get Building
            var building = _context.Building.Find(id);

            if(building == null)
            {
                return NotFound(); 
            }

            return View(building);
        }

        //Edit Field POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Building building)
        {

            if (ModelState.IsValid)
            {
                
                _context.Building.Update(building);
                _context.SaveChanges();

                TempData["success"] = "The building has been updated";
                return RedirectToAction("Index");
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
            var building = _context.Building.Find(id);

            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }
        
        //Delete Field POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBuilding(Building building)
        {

            
            if (building == null)
            {
                return NotFound();
            }

            //var obj = new RoomsController();
              
            _context.Building.Remove(building);

            IEnumerable<Room> rooms = _context.Room;

            foreach (Room room in rooms)
            {
                if (room.BuildingID == building.BuildingID)
                {
                    _context.Room.Remove(room);//Removing rooms from this building
                    IEnumerable<Reservation> reservs = _context.Reservation;

                    foreach (Reservation reserv in reservs)
                    {
                        if (reserv.RoomID == room.RoomID)
                        {
                            _context.Reservation.Remove(reserv);//Removing reservations from existing rooms on this building
                        }
                    }
                }
            }

            _context.SaveChanges();

            TempData["success"] = "The building has been removed";
            return RedirectToAction("Index");
            

        }
    }
}
