using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcess.Entities;
using DataAcess.Interfaces;

namespace DataAcess.Repositories
{
   public class UserRepository : IUserRepository
    {
        public EF.ApplicationContext Db { get; set; }

        public UserRepository(EF.ApplicationContext db)
        {
            Db = db;
        }


        public List<ApplicationUser> GetUsers()
        {
            return Db.Users.ToList();
        }
    }
}
