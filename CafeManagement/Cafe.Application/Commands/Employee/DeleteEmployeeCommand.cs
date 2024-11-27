using MediatR;

namespace Cafe.Application.Commands.Employee
{
    public class DeleteEmployeeCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
