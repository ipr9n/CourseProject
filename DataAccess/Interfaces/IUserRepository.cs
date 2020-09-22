using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcess.Entities;

namespace DataAcess.Interfaces
{
    public interface IUserRepository
    {
       List<ApplicationUser> GetUsers();
    }
}
