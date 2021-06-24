using DevOPS_Project.Data;
using DevOPS_Project.Helper;
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
    public class RoomsController : Controller
    {

        private readonly ApplicationDbContext _context;


        

        public RoomsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Rooms Index 
        public IActionResult Index(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            IEnumerable<Room> listRoom = _context.Room;
            
            //Enviar valores a la vista
            ViewBag.Numerable = listRoom;
            ViewBag.BId = id;
            return View(listRoom);
        }

        [HttpGet]
        public async Task<IActionResult> GetData(int? id)//Get data for table
        {
            var all = await _context.Room
                .Where(e=> e.BuildingID == id)
                .Select(i => new
            {
                roomID = i.RoomID,
                roomName = i.RoomName,
                isTR = i.IsTR,
                syscreatedDt = i.SyscreatedDt.ToString("dd-MM-yyyy hh:mm:ss tt"),
                actions = "<a href='../../Rooms/Edit/" + i.RoomID + "' class='btn btn-warning'>Update</a> " +
                "<a href='../../Rooms/Delete/" + i.RoomID + "' class='btn btn-danger'>Delete</a> " +
                "<a href='../../Rooms/Calendar/" + i.RoomID + "' class='btn btn-dark'>Calendar</a>",
            }).ToListAsync();

            return Json(new { data = all });
        }

        //Get Calendar view for an specific Room
        public IActionResult Calendar(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            ViewBag.BId = id;
            return View();
        }

        public IActionResult GetRooms()//Get rooms
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

        public IActionResult GetEvents(int? id)//get events
        {
            
            var events = _context.Reservation
                .Where(i => i.RoomID == id)
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

            ViewBag.BId = id;
            return View();
        }

        //Create new reservations POST redirecting to rooms index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateReserv(Reservation reserv)
        {

            var control = false; //global control variable
            
            //Getting Building ID
            var BId = _context.Room
                .Where(i => i.RoomID == reserv.RoomID)
                .Select(x => x.BuildingID).Single();

            if (ModelState.IsValid)
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
                    _context.Reservation.Add(reserv);
                    _context.SaveChanges();

                    TempData["success"] = "The new reservation has been created";

                    return RedirectToAction("Index", new { id = BId });
                }
                else
                {
                    TempData["success"] = "There is a reservation on the schedule you selected";

                    return RedirectToAction("Index", new { id = BId });
                }

            }
            else
            {
                return NotFound();
            }


        }

        //Create new entries POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Room room)
        {
            if(ModelState.IsValid)
            {
                room.SyscreatedDt = DateTime.Now;
                _context.Room.Add(room);
                _context.SaveChanges();

                TempData["success"] = "The new room has been created";

                return RedirectToAction("Index", new { id = room.BuildingID });
            }

            return NotFound();
        }

        //Edit field GET
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            //Get Room
            var room = _context.Room.Find(id);

            if(room == null)
            {
                return NotFound(); 
            }

            return View(room);
        }

        //Edit Field POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Room room)
        {

            if (ModelState.IsValid)
            {
                var id = room.BuildingID;
                _context.Room.Update(room);
                _context.SaveChanges();

                TempData["success"] = "The room has been updated";
                return RedirectToAction("Index", new { id = id });
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
            IEnumerable<Reservation> reservs = _context.Reservation;

            foreach(Reservation reserv in reservs){
                if(reserv.RoomID == room.RoomID)
                {
                    _context.Reservation.Remove(reserv);//Removing reservation on this room
                }
            }
            _context.SaveChanges();

            TempData["success"] = "The room has been removed";
            return RedirectToAction("Index", new { id = BId });
            

        }

        //Edit field GET reservation update process
        public IActionResult EditReserv(int? id)
        {
            if (id == null || id == 0)
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

        //Edit Field POST reservation update process
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditReserv(Reservation reserv)
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

                        return RedirectToAction("Calendar", new { id = reserv.RoomID });
                    }
                    else
                    {
                        TempData["success"] = "There is a reservation on the schedule you selected";

                        return RedirectToAction("Calendar", new { id = reserv.RoomID });
                    }
                }
                else
                {
                    TempData["success"] = "You can't update this reservation";
                    return RedirectToAction("Calendar", new { id = reserv.RoomID });
                }
            }

            return View();
        }

    }
}
