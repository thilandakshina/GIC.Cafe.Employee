using Cafe.Application.Commands.Employee;
using Cafe.Application.Services;
using MediatR;

namespace Cafe.Application.Handlers.Employee
{
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, bool>
    {
        private readonly IEmployeeService _employeeService;

        public DeleteEmployeeCommandHandler(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<bool> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
        {
            return await _employeeService.DeleteAsync(command.Id);
        }
    }
}
