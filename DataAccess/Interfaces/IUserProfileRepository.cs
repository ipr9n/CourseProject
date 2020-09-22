using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcess.Entities;

namespace DataAcess.Interfaces
{
    public interface IUserProfileRepository
    {
       ClientProfile GetUserById(string id);
        void RemoveProfile(string userId);
    }
}
