using AutoMapper;
using GenericController.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace GenericController.Services
{
    public class GenericService<T, TDto> : IGenericService<T, TDto> where T : class where TDto : class
    {
        protected readonly IGenericRepository<T> _repository;
        protected readonly IMapper _mapper;
        protected readonly ILogger<GenericService<T, TDto>> _logger;

        public GenericService(IGenericRepository<T> repository, IMapper mapper, ILogger<GenericService<T, TDto>> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<TDto>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<TDto>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all entities of type {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public async Task<TDto?> GetByIdAsync(object id)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                return entity != null ? _mapper.Map<TDto>(entity) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving entity of type {EntityType} with id {Id}", typeof(T).Name, id);
                throw;
            }
        }

        public async Task<IEnumerable<TDto>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                var entities = await _repository.FindAsync(predicate);
                return _mapper.Map<IEnumerable<TDto>>(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding entities of type {EntityType} with predicate", typeof(T).Name);
                throw;
            }
        }

        public async Task<TDto> AddAsync(TDto dto)
        {
            try
            {
                var entity = _mapper.Map<T>(dto);
                var addedEntity = await _repository.AddAsync(entity);
                return _mapper.Map<TDto>(addedEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding entity of type {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public async Task UpdateAsync(object id, TDto dto)
        {
            try
            {
                var existingEntity = await _repository.GetByIdAsync(id);
                if (existingEntity == null)
                {
                    throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with id {id} not found.");
                }

                _mapper.Map(dto, existingEntity);
                await _repository.UpdateAsync(existingEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entity of type {EntityType} with id {Id}", typeof(T).Name, id);
                throw;
            }
        }

        public async Task DeleteAsync(object id)
        {
            try
            {
                await _repository.DeleteAsync(id);
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
                await _repository.SoftDeleteAsync(id);
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
                return await _repository.ExistsAsync(id);
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
                return await _repository.CountAsync(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error counting entities of type {EntityType}", typeof(T).Name);
                throw;
            }
        }

        public async Task<(IEnumerable<TDto> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            try
            {
                var query = _repository.GetQueryable();

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                var totalCount = await query.CountAsync();

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var dtos = _mapper.Map<IEnumerable<TDto>>(items);
                return (dtos, totalCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged entities of type {EntityType}", typeof(T).Name);
                throw;
            }
        }
    }
}