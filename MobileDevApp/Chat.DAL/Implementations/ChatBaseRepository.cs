using Chat.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Patterns.Specification.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Chat.DAL.Implementations
{
    public class ChatBaseRepository<T> : IChatRepository<T> where T : class
    {
        private readonly IChatDbContext _chatDbContext;
        private readonly Func<IChatDbContext, DbSet<T>> _dbTableSelector;

        public ChatBaseRepository(IChatDbContext chatDbContext,
            Func<IChatDbContext, DbSet<T>> dbTableSelector)
        {
            _chatDbContext = chatDbContext ?? throw new ArgumentNullException(nameof(chatDbContext));
            _dbTableSelector = dbTableSelector ?? throw new ArgumentNullException(nameof(dbTableSelector));
        }

        public int Create(T entity)
        {
            _dbTableSelector(_chatDbContext).Add(entity);
            return _chatDbContext.SaveChanges();
        }

        public Task<int> CreateAsync(T entity)
        {
            _dbTableSelector(_chatDbContext).Add(entity);
            return _chatDbContext.SaveChangesAsync();
        }

        public T Delete(ISpecification<T> deleteSpecification)
        {
            T toDelete;

            DbSet<T> dbTable = _dbTableSelector(_chatDbContext);
            toDelete = dbTable.FirstOrDefault(o => deleteSpecification.IsSatisfiedBy(o));

            if (toDelete != default(T))
            {
                dbTable.Remove(toDelete);
                _chatDbContext.SaveChanges();
            }

            return toDelete;
        }

        public Task DeleteAsync(ISpecification<T> deleteSpecification)
        {
            DbSet<T> dbTable = _dbTableSelector(_chatDbContext);
            T toDelete = dbTable.FirstOrDefault(o => deleteSpecification.IsSatisfiedBy(o));

            if (toDelete != default(T))
                dbTable.Remove(toDelete);

            return _chatDbContext.SaveChangesAsync();
        }

        public List<T> FindAll(ISpecification<T> specification = null,
            Func<IQueryable<T>, IQueryable<T>> includer = null)
        {
            return GetQueryToFind(specification, includer).ToList();
        }

        public Task<List<T>> FindAllAsync(ISpecification<T> specification = null,
            Func<IQueryable<T>, IQueryable<T>> includer = null)
        {
            return GetQueryToFind(specification, includer).ToListAsync();
        }

        public T FirstOrDefault(ISpecification<T> specification,
            Func<IQueryable<T>, IQueryable<T>> includer = null)
        {
            return GetQueryToFind(specification, includer).FirstOrDefault();
        }

        public Task<T> FirstOrDefaultAsync(ISpecification<T> specification,
            Func<IQueryable<T>, IQueryable<T>> includer = null)
        {
            return GetQueryToFind(specification, includer).FirstOrDefaultAsync();
        }

        public int Update(T entity)
        {
            _dbTableSelector(_chatDbContext).Attach(entity);
            _chatDbContext.Entry(entity).State = EntityState.Modified;
            return _chatDbContext.SaveChanges();
        }

        public Task<int> UpdateAsync(T entity)
        {
            _dbTableSelector(_chatDbContext).Attach(entity);
            _chatDbContext.Entry(entity).State = EntityState.Modified;
            return _chatDbContext.SaveChangesAsync();
        }

        #region Private

        private IQueryable<T> GetQueryToFind(ISpecification<T> specification,
            Func<IQueryable<T>, IQueryable<T>> includer)
        {
            IQueryable<T> toFind = _dbTableSelector(_chatDbContext);

            if (includer != null)
                toFind = includer(toFind);

            if (specification != null)
                toFind = toFind.Where(o => specification.IsSatisfiedBy(o));

            return toFind;
        }

        #endregion
    }
}
