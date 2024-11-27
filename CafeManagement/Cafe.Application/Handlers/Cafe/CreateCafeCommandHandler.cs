using AutoMapper;
using Cafe.Application.Commands.Cafe;
using Cafe.Application.Services;
using Cafe.Domain.Entities;
using MediatR;

namespace Cafe.Application.Handlers.Cafe
{
    public class CreateCafeCommandHandler : IRequestHandler<CreateCafeCommand, Guid>
    {
        private readonly ICafeService _cafeService;
        private readonly IMapper _mapper;

        public CreateCafeCommandHandler(ICafeService cafeService, IMapper mapper)
        {
            _cafeService = cafeService;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateCafeCommand command, CancellationToken cancellationToken)
        {
            var cafe = _mapper.Map<CafeEntity>(command);
            return await _cafeService.CreateAsync(cafe);
        }
    }
}
