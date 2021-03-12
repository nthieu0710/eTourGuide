using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading.Tasks;
using eTourGuide.Data.Context;

namespace eTourGuide.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private static eTourGuideContext Context;
        private static DbSet<T> Table { get; set; }

        public GenericRepository(eTourGuideContext context)
        {
            Context = context;
            Table = Context.Set<T>();
        }

        public T Find(Func<T, bool> predicate)
        {
            return Table.FirstOrDefault(predicate);
        }

        public IQueryable<T> FindAll(Func<T, bool> predicate)
        {
            return Table.Where(predicate).AsQueryable();
        }

        public DbSet<T> GetAll()
        {
            return Table;
        }

        public T GetByIdGuid(Guid Id)
        {
            return Table.Find(Id);
        }
        public T GetById(int Id)
        {
            return Table.Find(Id);
        }
        public void HardDeleteGuid(Guid key)
        {
            Table.Remove(GetByIdGuid(key));
        }
        public void HardDelete(int key)
        {
            Table.Remove(GetById(key));
        }
        public void Insert(T entity)
        {
            Table.Add(entity);
        }

        public void UpdateGuid(T entity, Guid Id)
        {
            var existEntity = GetByIdGuid(Id);
            Context.Entry(existEntity).CurrentValues.SetValues(entity);
            Table.Update(existEntity);
        }
        public void Update(T entity, int Id)
        {
            var existEntity = GetById(Id);
            Context.Entry(existEntity).CurrentValues.SetValues(entity);
            Table.Update(existEntity);
        }
        public void UpdateRange(IQueryable<T> entities)
        {
            Table.UpdateRange(entities);
        }

        public void DeleteRange(IQueryable<T> entities)
        {
            Table.RemoveRange(entities);
        }

        public void InsertRange(IQueryable<T> entities)
        {
            Table.AddRange(entities);
        }
        //async
        public async Task InsertAsync(T entity)
        {
            await Table.AddAsync(entity);
        }
              
        public async Task InsertRangeAsync(IQueryable<T> entities)
        {
            await Table.AddRangeAsync(entities);
        }

        public EntityEntry<T> Delete(T entity)
        {
            return Table.Remove(entity);
        }
    }
}