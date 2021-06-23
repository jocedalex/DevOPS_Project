using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevOPS_Project.Models
{   //Buildings data base method
    public class ToolUser
    {
        //Required primary key
        [Key]
        [Required(ErrorMessage = "The ID is required")]
        public int UserID { get; set; }

        //Name varchar(50)
        [Required(ErrorMessage = "The name is required")]
        [StringLength(30,ErrorMessage = "The name can't have more than 30 characters")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        
        [Required(ErrorMessage = "The date is required")]
        [Display(Name = "Creation Date")]
        public DateTime SyscreatedDt { get; set; }
    }
}
