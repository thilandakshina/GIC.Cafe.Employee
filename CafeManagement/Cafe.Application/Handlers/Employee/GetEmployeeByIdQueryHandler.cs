using AutoMapper;
using Cafe.Application.DTOs;
using Cafe.Application.Queries.Employee;
using Cafe.Application.Services;
using Cafe.Application.Utils;
using MediatR;

namespace Cafe.Application.Handlers.Employee
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public GetEmployeeByIdQueryHandler(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
        {
            var employeeData = await _employeeService.GetByIdAsync(query.Id);
            if (employeeData.Employee == null)
                return null;

            var employeeDto = _mapper.Map<EmployeeDto>(employeeData.Employee);
            if (employeeData.AssignedCafe != null)
            {
                employeeDto.CafeId = employeeData.AssignedCafe.Id;
                employeeDto.CafeName = employeeData.AssignedCafe.Name;
            }
            employeeDto.DaysWorked = EmployeeCalculations.CalculateTotalDaysWorked(employeeData.CafeEmployments);

            return employeeDto;
        }
    }
}