using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class GameTag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Game> Games { get; set; }

        public GameTag()
        {
            Games = new List<Game>();
        }
    }
}
