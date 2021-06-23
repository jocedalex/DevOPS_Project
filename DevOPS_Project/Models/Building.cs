using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevOPS_Project.Models
{   //Buildings data base method
    public class Building
    {
        //Required primary key
        [Key]
        [Required(ErrorMessage = "The ID is required")]
        public int BuildingID { get; set; }

        //Name varchar(50)
        [Required(ErrorMessage = "The name is required")]
        [StringLength(50,ErrorMessage = "The name can't have more than 50 characters")]
        [Display(Name = "Building Name")]
        public string BuildingName { get; set; }

        
        [Required(ErrorMessage = "The date is required")]
        [Display(Name = "Creation Date")]
        public DateTime SyscreatedDt { get; set; }
    }
}
