using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogicLayer.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "name", ResourceType = typeof(ViewResources.Resources))]
        public string Name { get; set; }

        [Display(Name = "age", ResourceType = typeof(ViewResources.Resources))]
        public int Age { get; set; }

        [Display(Name = "city", ResourceType = typeof(ViewResources.Resources))]
        public string Address { get; set; }

        [Display(Name = "gender", ResourceType = typeof(ViewResources.Resources))]
        public string Gender { get; set; }

        public string AboutMe { get; set; }

        public string Email { get; set; }
        public string AvatarUrl { get; set; }

        public string Status { get; set; }
        public string Role { get; set; }
    }
}