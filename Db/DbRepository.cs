using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Demo.Db
{
    public class DbRepository<T> : IDbRepository<T> where T : class
    {
        private readonly DemoContext _context;
        private readonly DbSet<T> _dbSet;

        public DbRepository(DemoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<T> GetAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var entity = await GetAsync(id);

            if (entity is null)
                return;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // This method is for updates, so you get your entity, make your changes, then save
        public async Task SaveAsync(T entity)
        {
            await _context.SaveChangesAsync();
        }
    }
}