using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LogicLayer.Enums;

namespace LogicLayer.ViewModels
{
    public class CollectionViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "collectionName", ResourceType = typeof(ViewResources.Resources))]
        public string CollectionName { get; set; }
        [Required]
        [Display(Name = "collectionDescription", ResourceType = typeof(ViewResources.Resources))]
        public string CollectionDescription { get; set; }

        public string CollectionCreatorId { get; set; }
        [Required]
        [Display(Name = "collectionType", ResourceType = typeof(ViewResources.Resources))]
        public string CollectionType { get; set; }

        public string ImageUrl { get; set; }

        public List<CustomFieldViewModel> CustomFieldViewModels { get; set; }

        public List<ItemViewModel> CollectionItems { get; set; }


        public string CreatorName { get; set; }
        // public List<GroupPostViewModel> GroupPosts { get; set; }
        //   public byte[] Avatar { get; set; }

        public CollectionViewModel()
        {
            CustomFieldViewModels = new List<CustomFieldViewModel>();
            
            CollectionItems = new List<ItemViewModel>();
        }
    }
}