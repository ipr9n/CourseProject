using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.ViewModels
{
    public class CollectionSearchViewModel
    {
       public int Id { get; set; }
        public string CollectionName { get; set; }

        public List<ItemViewModel> CollectionItems { get; set; }
    }
}
