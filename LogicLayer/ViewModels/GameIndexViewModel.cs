using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace LogicLayer.ViewModels
{
    public class GameIndexViewModel
    {
        public List<TagViewModel> Tags { get; set; }
        public List<GameViewModel> Games { get; set; }
    }
}
