using System.Configuration;
using LogicLayer.Interfaces;
using DataAcess.Repositories;
using DataAcess.Identity;

namespace LogicLayer.Services
{
    public class ServiceCreator : IServiceCreator
    {
        readonly string connection = ConfigurationManager.AppSettings["connectionString"];

        public IUserService CreateUserService()
        {
            return new UserService(new IdentityUnitOfWork(connection));
        }

        public ICollectionService CreateCollectionService()
        {
            return new CollectionService(new IdentityUnitOfWork(connection));
        }

  
    }
}
