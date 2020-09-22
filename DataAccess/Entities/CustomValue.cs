using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class CustomValue
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int ItemId { get; set; }
        public int CustomFieldId { get; set; }
    }
}
