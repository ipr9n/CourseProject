using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Item> Items { get; set; }

        public Tag()
        {
            Items = new List<Item>();
        }
    }
}
