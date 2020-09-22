using DataAcess.Entities;
using System;

namespace DataAcess.Interfaces
{
    public interface IClientManager:IDisposable
    {
        void Create(ClientProfile item);
    }
}
