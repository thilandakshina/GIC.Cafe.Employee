using MediatR;
using static Cafe.Application.Common;

namespace Cafe.Application.Commands.Employee
{
    public record CreateEmployeeCommand : IRequest<Guid>
    {
        public string Name { get; init; }
        public string EmailAddress { get; init; }
        public string PhoneNumber { get; init; }
        public GenderType Gender { get; set; } = GenderType.Male;
        public Guid? CafeId { get; init; }
    }
}
