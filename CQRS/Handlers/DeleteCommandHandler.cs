using GenericController.CQRS.Commands;
using GenericController.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace GenericController.CQRS.Handlers
{
    public class DeleteCommandHandler<T> : IRequestHandler<DeleteCommand<T>> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly ILogger<DeleteCommandHandler<T>> _logger;

        public DeleteCommandHandler(IGenericRepository<T> repository, ILogger<DeleteCommandHandler<T>> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(DeleteCommand<T> request, CancellationToken cancellationToken)
        {
            try
            {
                await _repository.DeleteAsync(request.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity of type {EntityType} with id {Id}", typeof(T).Name, request.Id);
                throw;
            }
        }
    }
}