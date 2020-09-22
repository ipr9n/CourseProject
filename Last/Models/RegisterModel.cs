using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Last.Models
{
    public class RegisterModel
    {
        [Required]
        [Display(Name = "email", ResourceType = typeof(Resources.Resources))]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "password", ResourceType = typeof(Resources.Resources))]
        public string Password { get; set; }
        [Display(Name = "repeatPassword", ResourceType = typeof(Resources.Resources))]
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "firstName", ResourceType = typeof(Resources.Resources))]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "secondName", ResourceType = typeof(Resources.Resources))]
        [Required]
        public string SecondName { get; set; }
        [Display(Name = "age", ResourceType = typeof(Resources.Resources))]
        [Required]
        public int Age { get; set; }
        [Display(Name = "city", ResourceType = typeof(Resources.Resources))]
        [Required]
        public string Address { get; set; }
        [Display(Name = "gender", ResourceType = typeof(Resources.Resources))]
        [Required]
        public string Gender { get; set; }
        public string AvatarUrl { get; set; }
    }
}
