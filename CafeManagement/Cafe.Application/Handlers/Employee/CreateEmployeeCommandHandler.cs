using AutoMapper;
using Cafe.Application.Commands.Employee;
using Cafe.Application.Services;
using Cafe.Domain.Entities;
using MediatR;

namespace Cafe.Application.Handlers.Employee
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Guid>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public CreateEmployeeCommandHandler(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employee = _mapper.Map<EmployeeEntity>(command);
            return await _employeeService.CreateAsync(employee, command.CafeId);
        }
    }
}
