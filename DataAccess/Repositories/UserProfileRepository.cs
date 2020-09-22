using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcess.Entities;
using DataAcess.Interfaces;

namespace DataAcess.Repositories
{
   public class UserProfileRepository : IUserProfileRepository
    {
        public EF.ApplicationContext Db { get; set; }

        public UserProfileRepository(EF.ApplicationContext db)
        {
            Db = db;
        }

        public ClientProfile GetUserById(string id) => Db.Users.FirstOrDefault(x => x.Id == id).ClientProfile;

        public void RemoveProfile(string userId)
        {
            Db.ClientProfiles.Remove(GetUserById(userId));
            Db.SaveChanges();
        }
    }
}
