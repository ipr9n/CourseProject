using System.Collections.Generic;
using DataAccess.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAcess.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
        public virtual List<ItemCollection> ItemCollections { get; set; }
        public virtual List<ItemComment> ItemComments { get; set; }
        public bool IsBlocked { get; set; }

        public ApplicationUser()
        {
            ItemComments = new List<ItemComment>();
            ItemCollections = new List<ItemCollection>();
        }
    }
}
