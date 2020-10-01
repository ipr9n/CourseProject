using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
   public  class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PlayersCount { get; set; }

        public virtual List<GameTag> GameTags { get; set; }

        public Game()
        {
            GameTags = new List<GameTag>();
        }
    }
}
