using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevOPS_Project.Models.ViewModel
{
    public class UserVM
    {

        [Required(ErrorMessage = "The name is required")]
        [StringLength(30, ErrorMessage = "The name can't have more than 30 characters")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

    }
}
