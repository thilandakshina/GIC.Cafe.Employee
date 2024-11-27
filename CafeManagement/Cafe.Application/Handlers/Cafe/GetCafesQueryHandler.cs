using AutoMapper;
using Cafe.Application.DTOs;
using Cafe.Application.Queries.Cafe;
using Cafe.Application.Services;
using MediatR;

namespace Cafe.Application.Handlers.Cafe
{
    public class GetCafesQueryHandler : IRequestHandler<GetCafesQuery, IEnumerable<CafeDto>>
    {
        private readonly ICafeService _cafeService;
        private readonly IMapper _mapper;

        public GetCafesQueryHandler(ICafeService cafeService, IMapper mapper)
        {
            _cafeService = cafeService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CafeDto>> Handle(GetCafesQuery query, CancellationToken cancellationToken)
        {
            var cafesWithEmployees = await _cafeService.GetAllAsync(query.Location);

            return cafesWithEmployees
                .Select(cafeWithEmployees =>
                {
                    var cafeDto = _mapper.Map<CafeDto>(cafeWithEmployees.Cafe);
                    cafeDto.EmployeeCount = cafeWithEmployees.Employees.Count();
                    return cafeDto;
                })
                .OrderByDescending(x => x.EmployeeCount);
        }
    }
}