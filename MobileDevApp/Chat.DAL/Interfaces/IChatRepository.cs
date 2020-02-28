using Patterns.Specification.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.DAL.Interfaces
{
    public interface IChatRepository<T> where T : class
    {
        int Create(T entity);

        Task<int> CreateAsync(T entity);

        T Delete(ISpecification<T> deleteSpecification);

        Task DeleteAsync(ISpecification<T> deleteSpecification);

        List<T> FindAll(ISpecification<T> specification = null,
            Func<IQueryable<T>, IQueryable<T>> includer = null);

        Task<List<T>> FindAllAsync(ISpecification<T> specification = null,
            Func<IQueryable<T>, IQueryable<T>> includer = null);

        T FirstOrDefault(ISpecification<T> specification,
            Func<IQueryable<T>, IQueryable<T>> includer = null);

        Task<T> FirstOrDefaultAsync(ISpecification<T> specification,
            Func<IQueryable<T>, IQueryable<T>> includer = null);

        int Update(T entity);

        Task<int> UpdateAsync(T entity);
    }
}
