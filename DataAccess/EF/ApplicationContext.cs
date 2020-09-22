using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using DataAcess.Entities;
using DataAccess.Entities;

namespace DataAcess.EF
{
    public class ApplicationContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string conectionString) : base(conectionString) { }

        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<ItemCollection> ItemCollections { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ItemComment> itemComments { get; set; }
        public DbSet<ItemLike> itemLikes { get; set; }
        public DbSet<CustomField> CustomFields { get; set; }

        static ApplicationContext()
        {
            Database.SetInitializer<ApplicationContext>(new DatabaseInitializer());
        }
    }
}
