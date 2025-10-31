using MediatR;

namespace GenericController.CQRS.Queries
{
    public class GetAllQuery<T> : IRequest<IEnumerable<T>>
    {
    }
}