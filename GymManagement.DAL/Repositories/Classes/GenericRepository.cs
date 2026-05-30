using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepository(GymDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        public async Task<int> AddAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.Add(entity);

            return await _context.SaveChangesAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default) 
            => _dbSet.AsNoTracking().AnyAsync(predicate, ct);

        public async Task<int> DeleteAsync(int id, CancellationToken ct = default)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return 0;
            }

            _dbSet.Remove(entity);

            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _dbSet : _dbSet.AsNoTracking();

            return await query.ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default) 
            => await _dbSet.FindAsync(id, ct);

        public async Task<int> UpdateAsync(TEntity entity, CancellationToken ct = default)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync(ct);
        }
    }
}
