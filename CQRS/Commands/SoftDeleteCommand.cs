using MediatR;

namespace GenericController.CQRS.Commands
{
    public class SoftDeleteCommand<T> : IRequest
    {
        public object Id { get; set; }

        public SoftDeleteCommand(object id)
        {
            Id = id;
        }
    }
}