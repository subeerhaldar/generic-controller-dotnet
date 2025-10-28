using GenericController.Data;
using GenericController.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace GenericController.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly ILogger<GenericRepository<T>> _logger;

        public GenericRepository(AppDbContext context, ILogger<GenericRepository<T>> logger)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _logger = logger;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all entities of type {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving entity of type {EntityType} with id {Id}", typeof(T).Name, id);
                throw;
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding entities of type {EntityType} with predicate", typeof(T).Name);
                throw;
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding entity of type {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entity of type {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public async Task DeleteAsync(object id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity of type {EntityType} with id {Id}", typeof(T).Name, id);
                throw;
            }
        }

        public async Task SoftDeleteAsync(object id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                if (entity is BaseEntity baseEntity)
                {
                    baseEntity.IsActive = false;
                    await UpdateAsync(entity);
                }
                else
                {
                    await DeleteAsync(id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting entity of type {EntityType} with id {Id}", typeof(T).Name, id);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(object id)
        {
            try
            {
                return await _dbSet.FindAsync(id) != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking existence of entity of type {EntityType} with id {Id}", typeof(T).Name, id);
                throw;
            }
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            try
            {
                if (predicate == null)
                    return await _dbSet.CountAsync();
                else
                    return await _dbSet.CountAsync(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting entities of type {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public IQueryable<T> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }
    }
}