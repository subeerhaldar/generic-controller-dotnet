using AutoMapper;
using GenericController.CQRS.Commands;
using GenericController.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GenericController.CQRS.Handlers
{
    public class CreateCommandHandler<T, TDto> : IRequestHandler<CreateCommand<T, TDto>, TDto> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCommandHandler<T, TDto>> _logger;

        public CreateCommandHandler(IGenericRepository<T> repository, IMapper mapper, ILogger<CreateCommandHandler<T, TDto>> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<TDto> Handle(CreateCommand<T, TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _mapper.Map<T>(request.Dto);
                var addedEntity = await _repository.AddAsync(entity);
                return _mapper.Map<TDto>(addedEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating entity of type {EntityType}", typeof(T).Name);
                throw;
            }
        }
    }
}