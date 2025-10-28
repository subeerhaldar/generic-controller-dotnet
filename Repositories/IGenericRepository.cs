using System.Linq.Expressions;

namespace GenericController.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(object id);
        Task SoftDeleteAsync(object id);
        Task<bool> ExistsAsync(object id);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        IQueryable<T> GetQueryable();
    }
}