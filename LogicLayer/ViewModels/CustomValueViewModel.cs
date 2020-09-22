using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.ViewModels
{
    public class CustomValueViewModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int ItemId { get; set; }
        public int CustomFieldId { get; set; }
    }
}
