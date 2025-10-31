using GenericController.CQRS.Commands;
using GenericController.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GenericController.CQRS.Handlers
{
    public class SoftDeleteCommandHandler<T> : IRequestHandler<SoftDeleteCommand<T>> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly ILogger<SoftDeleteCommandHandler<T>> _logger;

        public SoftDeleteCommandHandler(IGenericRepository<T> repository, ILogger<SoftDeleteCommandHandler<T>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(SoftDeleteCommand<T> request, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.SoftDeleteAsync(request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting entity of type {EntityType} with id {Id}", typeof(T).Name, request.Id);
                throw;
            }
        }
    }
}