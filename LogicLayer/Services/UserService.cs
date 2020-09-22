using LogicLayer.DTO;
using LogicLayer.Infrastructure;
using DataAcess.Entities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using LogicLayer.Interfaces;
using DataAcess.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using LogicLayer.ViewModels;
using DataAcess.Identity;

namespace LogicLayer.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            ApplicationUser user = await Database
                .UserManager
                .FindByEmailAsync(userDto.Email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = userDto.Email,
                    UserName = userDto.Email,
                    IsBlocked = false
                };

                var result = await Database
                    .UserManager
                    .CreateAsync(user, userDto.Password);

                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                // добавляем роль
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);

                // создаем профиль клиента
                ClientProfile userProfile = new ClientProfile()
                {
                    Id = user.Id,
                    FirstName = userDto.FirstName,
                    SecondName = userDto.SecondName,
                    AvatarUrl = userDto.AvatarUrl,
                    Address = userDto.Address,
                    Age = userDto.Age,
                    Gender = userDto.Gender
                };

                Database.ClientManager.Create(userProfile);

                await Database.SaveAsync();

                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public async Task<OperationDetails> AddUserToRole(string userid, string roleName)
        {
            await Database.UserManager.AddToRoleAsync(userid, roleName);
            await Database.SaveAsync();

            return new OperationDetails(true, "Пользователь получил роль", "");
        }

        public async Task<OperationDetails> RemoveUserRole(string userid, string roleName)
        {
            await Database.UserManager.RemoveFromRoleAsync(userid, roleName);
            await Database.SaveAsync();

            return new OperationDetails(true, "Пользователь получил роль", "");
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;

            // находим пользователя
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);

            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await Database
                    .UserManager
                    .CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            return claim;
        }

        // начальная инициализация бд
        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }

            await Create(adminDto);
        }

        public bool IsUserExist(string id) => Database.UserManager.FindById(id) != null;

        public UserViewModel GetProfileInformation(string id)
        {
            if (id == null)
            {
                return new UserViewModel()
                {
                    AboutMe = "Гость",
                    Address = "Гость",
                    Age = 0,
                    Gender = "Неизвестно",
                    Name = "Неизвестно"
                };
            }
            else
            {
                var userProfile = Database.UserProfileRepository.GetUserById(id);
                return new UserViewModel()
                {
                    Id = userProfile.Id,
                    AboutMe = userProfile.AboutMe,
                    Address = userProfile.Address,
                    Age = userProfile.Age,
                    Gender = userProfile.Gender,
                    AvatarUrl = userProfile.AvatarUrl,
                    Name = $"{userProfile.FirstName} {userProfile.SecondName}"
                };
            }
        }

        public List<UserViewModel> GetUsersProfiles()
        {
            List<UserViewModel> outputList = new List<UserViewModel>();
            var users = Database.UserManager.Users.ToList();

            users.ForEach(x => outputList.Add(new UserViewModel()
            {
                AboutMe = x.ClientProfile.AboutMe,
                Address = x.ClientProfile.Address,
                Age = x.ClientProfile.Age,
                AvatarUrl = x.ClientProfile.AvatarUrl,
                Gender = x.ClientProfile.Gender,
                Id = x.Id,
                Name = $"{x.ClientProfile.FirstName} {x.ClientProfile.SecondName}",
                Status = Database.UserManager.IsInRole(x.Id, "blocked")
                ? "Blocked"
                : "Active",
                Email = x.Email,
                Role = Database.UserManager.IsInRole(x.Id, "admin")
                ? "Admin"
                : "User"
            }));

            return outputList;
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public async Task<OperationDetails> BlockUsers(string[] usersId)
        {
            foreach (var userId in usersId)
            {

                var user = Database.UserManager.FindById(userId);
                if (user != null)
                {
                    await AddUserToRole(userId, "blocked");
                    user.IsBlocked = true;
                    await Database.SaveAsync();
                }
            }
            return new OperationDetails(true, "Okay", "");
        }

        public void DeleteUsers(string[] usersId)
        {
            foreach (var userId in usersId)
            {
                var user = Database.UserManager.FindById(userId);

                if (user != null)
                {
                    Database.UserProfileRepository.RemoveProfile(user.Id);
                    user.ItemCollections.RemoveAll(x => x.Id != -1);
                    Database.CollectionRepository.RemoveNullCollections();
                    Database.UserManager.Delete(user);
                }

                Database.SaveAsync();
            }
        }

        public bool IsBlocked(string userId)
        {
            return IsUserExist(userId) && Database.UserManager.FindById(userId).IsBlocked;
        }

        public async Task<OperationDetails> UnBlockUsers(string[] usersId)
        {
            foreach (var userId in usersId)
            {
                var user = Database.UserManager.FindById(userId);

                if (user != null)
                {
                    await RemoveUserRole(userId, "blocked");
                    user.IsBlocked = false;
                    await Database.SaveAsync();
                }

            }
            return new OperationDetails(true, "Okay", "");
        }

        public async Task<OperationDetails> AddUsersToRole(string[] id, string roleName)
        {
            foreach (var userId in id)
            {
                var user = Database.UserManager.FindById(userId);

                if (user != null)
                {
                    await AddUserToRole(userId, "admin");
                }
            }

            return new OperationDetails(true, "Okay", "");
        }

        public async Task<OperationDetails> RemoveUsersRole(string[] id, string roleName)
        {
            foreach (var userId in id)
            {
                var user = Database.UserManager.FindById(userId);

                if (user != null)
                {
                    await RemoveUserRole(userId, "admin");
                }
            }

            return new OperationDetails(true, "Okay", "");
        }

        public ApplicationUserManager GetApplicationUserManager()
        {
            return Database.UserManager;
        }
    }


}
