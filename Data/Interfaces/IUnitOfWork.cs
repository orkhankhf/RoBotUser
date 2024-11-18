using Entities.Models;

namespace Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> GetRepository<T>() where T : class; // New generic method
        Task<int> SaveChangesAsync();
    }
}
