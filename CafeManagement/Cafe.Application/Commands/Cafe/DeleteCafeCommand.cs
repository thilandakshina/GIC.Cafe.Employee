using MediatR;

namespace Cafe.Application.Commands.Cafe
{
    public class DeleteCafeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
