using MediatR;

namespace GenericController.CQRS.Commands
{
    public class CreateCommand<T, TDto> : IRequest<TDto>
    {
        public TDto Dto { get; set; }

        public CreateCommand(TDto dto)
        {
            Dto = dto;
        }
    }
}