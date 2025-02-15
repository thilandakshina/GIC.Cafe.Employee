using Cafe.Domain.Entities;
using Cafe.Infrastructure.Repositories.Interfaces;

namespace Cafe.Infrastructure.Repositories.Commands
{
    public interface IEmployeeCommandRepository : ICommandRepository<EmployeeEntity>
    {
        Task DeactivateEmployeeAsync(Guid employeeId);
        Task<bool> ExistsAsync(Guid id);
    }
}
