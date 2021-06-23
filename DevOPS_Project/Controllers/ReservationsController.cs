using DevOPS_Project.Data;
using DevOPS_Project.Helper;
using DevOPS_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevOPS_Project.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {

        private readonly ApplicationDbContext _context;


        

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Rooms Index 
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult GetRooms()//get rooms
        {
            var rooms = _context.Room
                .Select(e => new
                {
                    id = e.RoomID,
                    name = e.RoomName
                }
            ).ToList();


            return new JsonResult(rooms);
        }
        
        public IActionResult GetEvents()//get events
        {
            int UserID = int.Parse(SessionHelper.GetNameIdentifier(User));
            var events = _context.Reservation
                .Where(i=>i.UserID == UserID)
                .Select(e => new
            {
                id = e.ReservID,
                title = SessionHelper.GetName(User),
                start = e.ReservDT.ToString("yyyy-MM-dd") + " " + e.ReservStartTime.ToString("T"),
                end = e.ReservDT.ToString("yyyy-MM-dd") + " " + e.ReservEndTime.ToString("T"),
                allDay = "false",
            }
            ).ToList();
            
            return new JsonResult(events);
        }

        


        //Create new entries GET
        public IActionResult Create(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var room = _context.Room.Find(id);
            ViewBag.BId = id;
            ViewBag.Name = room.RoomName;
            return View();
        }

        //Create new entries POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Reservation reserv)
        {

            var control=false; //global control variable
            if(ModelState.IsValid)
            {
                var reservs = _context.Reservation
                    .Where(x => x.ReservDT == reserv.ReservDT && x.RoomID == reserv.RoomID)
                    .Select(e => new
                    {
                        start = e.ReservStartTime,
                        end = e.ReservEndTime,
                    }).ToList();//Selecting existing reservations for date

                foreach (var reservation in reservs)
                {
                    //Validation to avoid overlapping on reservations
                    if(reservation.start <= reserv.ReservStartTime && reservation.end > reserv.ReservStartTime || 
                        reservation.start < reserv.ReservEndTime && reservation.end >= reserv.ReservEndTime ||
                        reservation.start <= reserv.ReservStartTime && reservation.end >= reserv.ReservEndTime ||
                        reservation.start >= reserv.ReservStartTime && reservation.end <= reserv.ReservEndTime)
                    {
                        control = true;
                                                
                    }
                }

                if (!control)
                {
                    reserv.SyscreatedDt = DateTime.Now;
                    _context.Reservation.Add(reserv);
                    _context.SaveChanges();

                    TempData["success"] = "The new reservation has been created";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["success"] = "There is a reservation on the schedule you selected";
                    
                    return RedirectToAction("Index");
                }
                
            }
            else
            {
                return NotFound();
            }

            
        }

        
        //Edit field GET
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            //Get Reservation
            var reserv = _context.Reservation.Find(id);
            var room = _context.Room.Find(reserv.RoomID);
            ViewBag.Name = room.RoomName;

            if (reserv == null)
            {
                return NotFound(); 
            }

            return View(reserv);
        }

        //Edit Field POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Reservation reserv)
        {
            var control = false; //global control variable

            if (ModelState.IsValid)
            {
                if (reserv.UserID.ToString() == SessionHelper.GetNameIdentifier(User))
                {
                    var reservs = _context.Reservation
                    .Where(x => x.ReservDT == reserv.ReservDT && x.RoomID == reserv.RoomID && x.ReservID != reserv.ReservID)
                    .Select(e => new
                    {
                        start = e.ReservStartTime,
                        end = e.ReservEndTime,
                    }).ToList();//Selecting existing reservations for date

                    foreach (var reservation in reservs)
                    {
                        //Validation to avoid overlapping on reservations
                        if (reservation.start <= reserv.ReservStartTime && reservation.end > reserv.ReservStartTime ||
                            reservation.start < reserv.ReservEndTime && reservation.end >= reserv.ReservEndTime ||
                            reservation.start <= reserv.ReservStartTime && reservation.end >= reserv.ReservEndTime ||
                            reservation.start >= reserv.ReservStartTime && reservation.end <= reserv.ReservEndTime)
                        {
                            control = true;

                        }
                    }

                    if (!control)
                    {
                        reserv.SyscreatedDt = DateTime.Now;
                        _context.Reservation.Update(reserv);
                        _context.SaveChanges();

                        TempData["success"] = "The reservation has been updated";

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["success"] = "There is a reservation on the schedule you selected";

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["success"] = "You can't update this reservation";
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
            var room = _context.Room.Find(id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }
        
        //Delete Field POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteRoom(Room room)
        {

            
            if (room == null)
            {
                return NotFound();
            }
            var BId = room.BuildingID;
              
            _context.Room.Remove(room);
            _context.SaveChanges();

            TempData["success"] = "The room has been removed";
            return RedirectToAction("Index", new { id = BId });
            

        }
    }
}
