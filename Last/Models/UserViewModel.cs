using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Last.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Возраст")]
        public int Age { get; set; }

        [Display(Name = "Город")]
        public string Address { get; set; }

        [Display(Name = "Пол")]
        public string Gender { get; set; }

        [Display(Name = "Обо мне")]
        public string AboutMe { get; set; }

        public string Email { get; set; }

        public string Status { get; set; }
    }
}