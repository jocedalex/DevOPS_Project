using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DevOPS_Project.Models
{
    public class Reservation
    {

        //Required primary key
        [Key]
        [Required(ErrorMessage = "The ID is required")]
        public int ReservID { get; set; }

        //Foreign key for the Room ID
        [ForeignKey("RoomID")]
        [Required(ErrorMessage = "The Room ID is required")]
        public int RoomID { get; set; }

        //Foreign key for the User ID
        [ForeignKey("UserID")]
        [Required(ErrorMessage = "The User ID is required")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "The reservation date is required")]
        [Display(Name = "Reservation Date" )]
        public DateTime ReservDT { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Start Time")]
        public TimeSpan ReservStartTime { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "End Time")]
        public TimeSpan ReservEndTime { get; set; }

        [Required(ErrorMessage = "The date is required")]
        [Display(Name = "Creation Date")]
        public DateTime SyscreatedDt { get; set; }

    }
}
