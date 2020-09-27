using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.ViewModels
{
    public class CreateFieldViewModel
    {
       public int Id { get; set; }
       [Required]
       public string Name { get; set; }
       public string Type { get; set; }
        public int CollectionId { get; set; }
    }
}
