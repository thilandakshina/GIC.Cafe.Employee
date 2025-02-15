using Cafe.Application.DTOs;
using MediatR;

namespace Cafe.Application.Queries.Employee
{
    public record GetEmployeeByIdQuery : IRequest<EmployeeDto>
    {
        public Guid Id { get; init; }
    }
}
