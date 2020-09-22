using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.ViewModels
{
   public class ItemViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "itemname", ResourceType = typeof(ViewResources.Resources))]
        public string Name { get; set; }
        [Required]
        [Display(Name="description", ResourceType = typeof(ViewResources.Resources))]
        public string Description { get; set; }
        [Required]
        [Display(Name = "tags", ResourceType = typeof(ViewResources.Resources))]
        public string Tags { get; set; }
        public int GroupId { get; set; }

        public List<CustomValueViewModel> CustomValues { get; set; }
        public List<CustomFieldViewModel> CustomFieldViewModels { get; set; }


        public List<ItemLikeViewModel> ItemLikes { get; set; }

        public List<CommentViewModel> Comments { get; set; }

        public ItemViewModel()
        {
            CustomValues = new List<CustomValueViewModel>();
        }
    }
}
