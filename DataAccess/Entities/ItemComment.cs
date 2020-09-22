using DataAcess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class ItemComment
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
    }
}
