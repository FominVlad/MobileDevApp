using Chat.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Patterns.Specification.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<T> FindAll(ISpecification<T> specification = null,
            Func<IQueryable<T>, IQueryable<T>> includer = null)
        {
            return GetQueryToFind(specification, includer).ToList();
        }

        public T FirstOrDefault(ISpecification<T> specification,
            Func<IQueryable<T>, IQueryable<T>> includer = null)
        {
            return GetQueryToFind(specification, includer).FirstOrDefault();
        }

        public int Update(T entity)
        {
            _dbTableSelector(_chatDbContext).Attach(entity);
            _chatDbContext.Entry(entity).State = EntityState.Modified;
            return _chatDbContext.SaveChanges();
        }

        public List<T> TakeOrdered<TSelector>(
            ISpecification<T> specification,
            Func<IQueryable<T>, IQueryable<T>> includer,
            Func<T, TSelector> orderKeySelector,
            int? numbToTake = null,
            bool isDescending = false)
        {
            IEnumerable<T> allEntities = GetQueryToFind(specification, includer);

            IOrderedEnumerable<T> orderedFoundEntitites = isDescending ? 
                allEntities.OrderByDescending(orderKeySelector) : 
                allEntities.OrderBy(orderKeySelector);

            return numbToTake.HasValue ? 
                orderedFoundEntitites.Take(numbToTake.Value).ToList() : 
                orderedFoundEntitites.ToList();
        }

        #region Private

        private IEnumerable<T> GetQueryToFind(ISpecification<T> specification,
            Func<IQueryable<T>, IQueryable<T>> includer)
        {
            IQueryable<T> toFind = _dbTableSelector(_chatDbContext);

            if (includer != null)
                toFind = includer(toFind);

            IEnumerable<T> toReturn = toFind.AsEnumerable();
            if (specification != null)
                toReturn = toReturn.Where(o => specification.IsSatisfiedBy(o));

            return toReturn;
        }

        #endregion
    }
}
