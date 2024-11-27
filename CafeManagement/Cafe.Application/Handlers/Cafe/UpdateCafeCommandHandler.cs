using AutoMapper;
using Cafe.Application.Commands.Cafe;
using Cafe.Application.Services;
using Cafe.Domain.Entities;
using MediatR;

namespace Cafe.Application.Handlers.Cafe
{
    public class UpdateCafeCommandHandler : IRequestHandler<UpdateCafeCommand, bool>
    {
        private readonly ICafeService _cafeService;
        private readonly IMapper _mapper;

        public UpdateCafeCommandHandler(ICafeService cafeService, IMapper mapper)
        {
            _cafeService = cafeService;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateCafeCommand command, CancellationToken cancellationToken)
        {
            var cafe = _mapper.Map<CafeEntity>(command);
            return await _cafeService.UpdateAsync(cafe);
        }
    }
}
