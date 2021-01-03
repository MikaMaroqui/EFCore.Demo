using System;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Demo.Db
{
    public interface IDbRepository<T> where T : class
    {
        IQueryable<T> GetQueryable();

        Task<T> GetAsync(Guid id);

        Task AddAsync(T entity);

        Task RemoveAsync(Guid id);

        Task SaveAsync(T entity);
    }
}