using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name = "email", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "password", ResourceType = typeof(Resources.Resources))]
        public string Password { get; set; }
    }
}
