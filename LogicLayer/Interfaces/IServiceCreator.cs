namespace LogicLayer.Interfaces
{
    public interface IServiceCreator
    {
        IUserService CreateUserService();
        ICollectionService CreateCollectionService();
    }
}
