using Patterns.Specification.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.DAL.Interfaces
{
    public interface IChatRepository<T> where T : class
    {
        int Create(T entity);

        T Delete(ISpecification<T> deleteSpecification);

        List<T> FindAll(ISpecification<T> specification = null,
            Func<IQueryable<T>, IQueryable<T>> includer = null);

        T FirstOrDefault(ISpecification<T> specification,
            Func<IQueryable<T>, IQueryable<T>> includer = null);

        int Update(T entity);

        List<T> TakeOrdered<TSelector>(
            ISpecification<T> specification,
            Func<IQueryable<T>, IQueryable<T>> includer,
            Func<T, TSelector> orderKeySelector,
            int? numbToTake = null,
            bool isDescending = false);
    }
}
