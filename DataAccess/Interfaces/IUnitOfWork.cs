using DataAcess.Identity;
using System;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using DataAcess.Repositories;

namespace DataAcess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }
        IGameRepository GameRepository { get; }
        UserProfileRepository UserProfileRepository { get; }
        CollectionRepository CollectionRepository { get; }
        Task SaveAsync();
    }
}
