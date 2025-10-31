using MediatR;

namespace GenericController.CQRS.Queries
{
    public class GetPagedQuery<T> : IRequest<(IEnumerable<T> Items, int TotalCount)>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Filter { get; set; }

        public GetPagedQuery(int pageNumber, int pageSize, string? filter = null)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Filter = filter;
        }
    }
}