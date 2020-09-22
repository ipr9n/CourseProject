using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.Entities
{
    public class ItemCollection
    {
        [Key]
        public int Id { get; set; }
        public string CollectionName { get; set; }
        public string CollectionDescription { get; set; }
        public string CollectionType { get; set; }
        public string ApplicationUserId { get; set; }
        public string ImageUrl { get; set; }

        public virtual List<CustomField> CustomFields { get; set; }


        public virtual ApplicationUser ApplicationUser { get; set; }


        public virtual List<Item> Items { get; set; }

        public ItemCollection()
        {
            CustomFields = new List<CustomField>();
            Items = new List<Item>();
        }
    }
}
