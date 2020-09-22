using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public string AuthorName { get; set; }
        public string ImageUrl { get; set; }
    }
}
