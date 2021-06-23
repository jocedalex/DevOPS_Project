using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DevOPS_Project.Models
{   //Rooms data base method
    public class Room
    {
        //Required primary key
        [Key]
        [Required(ErrorMessage = "The ID is required")]
        public int RoomID { get; set; }
       
        //Foreign key for the building ID
        [ForeignKey("BuildingID")]
        [Required(ErrorMessage = "The ID is required")]
        public int BuildingID { get; set; }

        //Name varchar(50)
        [Required(ErrorMessage = "The name is required")]
        [StringLength(50,ErrorMessage = "The name can't have more than 50 characters")]
        [Display(Name = "Room Name")]
        public string RoomName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Training room")]
        public Boolean IsTR { get; set; }


        [Required(ErrorMessage = "The date is required")]
        [Display(Name = "Creation Date")]
        public DateTime SyscreatedDt { get; set; }
    }
}
