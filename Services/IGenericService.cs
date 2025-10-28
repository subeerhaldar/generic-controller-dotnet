using System.Linq.Expressions;

namespace GenericController.Services
{
    public interface IGenericService<T, TDto> where T : class where TDto : class
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto?> GetByIdAsync(object id);
        Task<IEnumerable<TDto>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<TDto> AddAsync(TDto dto);
        Task UpdateAsync(object id, TDto dto);
        Task DeleteAsync(object id);
        Task SoftDeleteAsync(object id);
        Task<bool> ExistsAsync(object id);
        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
        Task<(IEnumerable<TDto> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);
    }
}