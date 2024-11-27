using AutoMapper;
using Cafe.Application.Commands.Employee;
using Cafe.Application.Services;
using Cafe.Domain.Entities;
using MediatR;

namespace Cafe.Application.Handlers.Employee
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, bool>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public UpdateEmployeeCommandHandler(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employee = _mapper.Map<EmployeeEntity>(command);
            return await _employeeService.UpdateAsync(employee , command.CafeId);
        }
    }
}
