using AutoMapper;
using GenericController.CQRS.Queries;
using GenericController.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GenericController.CQRS.Handlers
{
    public class GetByIdQueryHandler<T, TDto> : IRequestHandler<GetByIdQuery<TDto>, TDto?> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetByIdQueryHandler<T, TDto>> _logger;

        public GetByIdQueryHandler(IGenericRepository<T> repository, IMapper mapper, ILogger<GetByIdQueryHandler<T, TDto>> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TDto?> Handle(GetByIdQuery<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                return entity != null ? _mapper.Map<TDto>(entity) : default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving entity of type {EntityType} with id {Id}", typeof(T).Name, request.Id);
                throw;
            }
        }
    }
}