using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace eTourGuide.Data.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        //async       
        Task InsertAsync(TEntity entity);    
        Task InsertRangeAsync(IQueryable<TEntity> entities);     
        DbSet<TEntity> GetAll();
        IQueryable<TEntity> FindAll(Func<TEntity, bool> predicate);
        TEntity Find(Func<TEntity, bool> predicate);
        TEntity GetById(int Id);
        TEntity GetByIdGuid(Guid Id);
        void Insert(TEntity entity);
        void Update(TEntity entity, int Id);
        void UpdateGuid(TEntity entity, Guid Id);
        void UpdateRange(IQueryable<TEntity> entities);
        void HardDelete(int key);
        void HardDeleteGuid(Guid key);
        void DeleteRange(IQueryable<TEntity> entities);
        void InsertRange(IQueryable<TEntity> entities);

        public EntityEntry<TEntity> Delete(TEntity entity);

    }
}