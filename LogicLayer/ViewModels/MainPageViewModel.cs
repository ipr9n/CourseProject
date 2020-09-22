using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.ViewModels
{
    public class MainPageViewModel
    {
        public List<CollectionViewModel> MaxItemCollections { get; set; }
        public List<ItemViewModel> LastItems { get; set; }
        public List<TagViewModel> Tags { get; set; }
    }
}
