using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAcess.Identity;
using LogicLayer.DTO;
using LogicLayer.Infrastructure;
using LogicLayer.ViewModels;

namespace LogicLayer.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task SetInitialData(UserDTO adminDto, List<string> roles);
        ApplicationUserManager GetApplicationUserManager();
        bool IsUserExist(string id);
        UserViewModel GetProfileInformation(string id);
        Task<OperationDetails> AddUserToRole(string userid, string roleName);
        void DeleteUsers(string[] id);
        bool IsBlocked(string userId);
        Task<OperationDetails> BlockUsers(string[] id);
        Task<OperationDetails> RemoveUserRole(string userid, string roleName);
        Task<OperationDetails> UnBlockUsers(string[] id);
        Task<OperationDetails> AddUsersToRole(string[] id, string roleName);
        Task<OperationDetails> RemoveUsersRole(string[] id, string roleName);
        List<UserViewModel> GetUsersProfiles();
    } 
}
