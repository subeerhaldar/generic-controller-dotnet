using MediatR;

namespace GenericController.CQRS.Queries
{
    public class GetByIdQuery<T> : IRequest<T?>
    {
        public object Id { get; set; }

        public GetByIdQuery(object id)
        {
            Id = id;
        }
    }
}