using AutoMapper;
using GenericController.CQRS.Queries;
using GenericController.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GenericController.CQRS.Handlers
{
    public class GetPagedQueryHandler<T, TDto> : IRequestHandler<GetPagedQuery<TDto>, (IEnumerable<TDto> Items, int TotalCount)> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPagedQueryHandler<T, TDto>> _logger;

        public GetPagedQueryHandler(IGenericRepository<T> repository, IMapper mapper, ILogger<GetPagedQueryHandler<T, TDto>> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<(IEnumerable<TDto> Items, int TotalCount)> Handle(GetPagedQuery<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _repository.GetQueryable();

                if (!string.IsNullOrEmpty(request.Filter))
                {
                    // Simple filter implementation - can be extended
                    query = query.Where(e => EF.Property<string>(e, "NoteCode").Contains(request.Filter) ||
                                             EF.Property<string>(e, "Description").Contains(request.Filter));
                }

                var totalCount = await query.CountAsync();

                var items = await query
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
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