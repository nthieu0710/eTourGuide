using System;
using System.Threading.Tasks;
using eTourGuide.Data.Repository;

namespace eTourGuide.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<T> Repository<T>()
          where T : class;
        int Commit();
        Task<int> CommitAsync();
    }
}