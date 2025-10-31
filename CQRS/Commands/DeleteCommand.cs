using MediatR;

namespace GenericController.CQRS.Commands
{
    public class DeleteCommand<T> : IRequest
    {
        public object Id { get; set; }

        public DeleteCommand(object id)
        {
            Id = id;
        }
    }
}