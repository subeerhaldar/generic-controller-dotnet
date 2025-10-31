using AutoMapper;
using GenericController.CQRS.Commands;
using GenericController.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GenericController.CQRS.Handlers
{
    public class UpdateCommandHandler<T, TDto> : IRequestHandler<UpdateCommand<T, TDto>> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCommandHandler<T, TDto>> _logger;

        public UpdateCommandHandler(IGenericRepository<T> repository, IMapper mapper, ILogger<UpdateCommandHandler<T, TDto>> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Handle(UpdateCommand<T, TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                var existingEntity = await _repository.GetByIdAsync(request.Id);
                if (existingEntity == null)
                {
                    throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with id {request.Id} not found.");
                }

                _mapper.Map(request.Dto, existingEntity);
                await _repository.UpdateAsync(existingEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating entity of type {EntityType} with id {Id}", typeof(T).Name, request.Id);
                throw;
            }
        }
    }
}