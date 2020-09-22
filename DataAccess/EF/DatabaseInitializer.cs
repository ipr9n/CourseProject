using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DataAcess.EF
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<ApplicationContext>
    {
        protected override void Seed(ApplicationContext db)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));

            var role1 = new IdentityRole() { Name = "admin" };
            var role2 = new IdentityRole() { Name = "user" };
            var role3 = new IdentityRole() { Name = "blocked" };

            roleManager.Create(role1);
            roleManager.Create(role2);
            roleManager.Create(role3);

            db.SaveChanges();
        }
    }
}
