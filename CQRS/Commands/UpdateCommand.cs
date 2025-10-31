using MediatR;

namespace GenericController.CQRS.Commands
{
    public class UpdateCommand<T, TDto> : IRequest
    {
        public object Id { get; set; }
        public TDto Dto { get; set; }

        public UpdateCommand(object id, TDto dto)
        {
            Id = id;
            Dto = dto;
        }
    }
}