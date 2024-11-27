using Cafe.Application.Commands.Cafe;
using Cafe.Application.Services;
using MediatR;

namespace Cafe.Application.Handlers.Cafe
{
    public class DeleteCafeCommandHandler : IRequestHandler<DeleteCafeCommand, bool>
    {
        private readonly ICafeService _cafeService;

        public DeleteCafeCommandHandler(ICafeService cafeService)
        {
            _cafeService = cafeService;
        }

        public async Task<bool> Handle(DeleteCafeCommand command, CancellationToken cancellationToken)
        {
            return await _cafeService.DeleteAsync(command.Id);
        }
    }

}
