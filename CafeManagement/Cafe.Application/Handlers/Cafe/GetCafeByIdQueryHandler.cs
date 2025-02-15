using AutoMapper;
using Cafe.Application.DTOs;
using Cafe.Application.Queries.Cafe;
using Cafe.Application.Services;
using MediatR;

namespace Cafe.Application.Handlers.Cafe
{
    public class GetCafeByIdQueryHandler : IRequestHandler<GetCafeByIdQuery, CafeDto>
    {
        private readonly ICafeService _cafeService;
        private readonly IMapper _mapper;

        public GetCafeByIdQueryHandler(ICafeService cafeService, IMapper mapper)
        {
            _cafeService = cafeService;
            _mapper = mapper;
        }

        public async Task<CafeDto> Handle(GetCafeByIdQuery query, CancellationToken cancellationToken)
        {
            var cafeInfo = await _cafeService.GetByIdAsync(query.Id);
            var cafeDto = _mapper.Map<CafeDto>(cafeInfo.Cafe);
            cafeDto.EmployeeCount = cafeInfo.Employees.Count();
            return cafeDto;
        }
    }
}