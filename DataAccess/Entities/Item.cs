using DataAcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual List<CustomValue> CustomValue { get; set; }

        public int ItemCollectionId { get; set; }
        public virtual ItemCollection ItemCollection { get; set; }

        public virtual List<ItemComment> ItemComments { get; set; }

        public virtual List<Tag> Tags { get; set; }

        public virtual List<ItemLike> ItemLikes { get; set; }

        public Item()
        {
            CustomValue = new List<CustomValue>();
            ItemLikes = new List<ItemLike>();
            ItemComments = new List<ItemComment>();
            Tags = new List<Tag>();
        }
    }
}
