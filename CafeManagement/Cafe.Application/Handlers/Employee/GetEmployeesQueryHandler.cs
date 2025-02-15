using AutoMapper;
using Cafe.Application.DTOs;
using Cafe.Application.Queries.Employee;
using Cafe.Application.Services;
using Cafe.Application.Utils;
using MediatR;

namespace Cafe.Application.Handlers.Employee
{
    public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeDto>>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public GetEmployeesQueryHandler(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDto>> Handle(GetEmployeesQuery query, CancellationToken cancellationToken)
        {
            var employeesWithDetails = await _employeeService.GetAllAsync(query.CafeId);

            var employeeDtos = employeesWithDetails.Select(employeeData =>
            {
                var employeeDto = _mapper.Map<EmployeeDto>(employeeData.Employee);
                if (employeeData.AssignedCafe != null)
                {
                    employeeDto.CafeId = employeeData.AssignedCafe.Id;
                    employeeDto.CafeName = employeeData.AssignedCafe.Name;
                }
                employeeDto.DaysWorked = EmployeeCalculations.CalculateTotalDaysWorked(employeeData.CafeEmployments);
                return employeeDto;
            });

            return employeeDtos.OrderByDescending(x => x.DaysWorked);
        }
    }
}