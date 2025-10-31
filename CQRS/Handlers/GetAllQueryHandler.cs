using AutoMapper;
using GenericController.CQRS.Queries;
using GenericController.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GenericController.CQRS.Handlers
{
    public class GetAllQueryHandler<T, TDto> : IRequestHandler<GetAllQuery<TDto>, IEnumerable<TDto>> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllQueryHandler<T, TDto>> _logger;

        public GetAllQueryHandler(IGenericRepository<T> repository, IMapper mapper, ILogger<GetAllQueryHandler<T, TDto>> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<TDto>> Handle(GetAllQuery<TDto> request, CancellationToken cancellationToken)
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
    }
}