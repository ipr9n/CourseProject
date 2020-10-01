using DataAcess.EF;
using DataAcess.Entities;
using DataAcess.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using DataAcess.Identity;

namespace DataAcess.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private ApplicationContext db;

        private ApplicationUserManager userManager;
        private UserProfileRepository userProfileRepository;
        private IGameRepository gameRepository;
        private CollectionRepository collectionRepository;
        private ApplicationRoleManager roleManager;
        private IClientManager clientManager;

        public IdentityUnitOfWork(string connectionString)
        {
            db = new ApplicationContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            gameRepository = new GameRepository(db);
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            collectionRepository = new CollectionRepository(db);
            userProfileRepository = new UserProfileRepository(db);
            clientManager = new ClientManager(db);
        }

        public IGameRepository GameRepository
        {
            get { return gameRepository; }
        }

        public ApplicationUserManager UserManager
        {
            get { return userManager; }
        }

        public CollectionRepository CollectionRepository
        {
            get { return collectionRepository; }
        }

        public UserProfileRepository UserProfileRepository
        {
            get { return userProfileRepository; }
        }

        public IClientManager ClientManager
        {
            get { return clientManager; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return roleManager; }
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    clientManager.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
