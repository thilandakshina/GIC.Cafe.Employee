using Cafe.Application.DTOs;
using MediatR;

namespace Cafe.Application.Queries.Employee
{
    public record GetEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>
    {
        public Guid? CafeId { get; init; }
    }
}
