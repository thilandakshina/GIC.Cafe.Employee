using Cafe.Domain.Entities;
using Cafe.Infrastructure.Repositories.Interfaces;

namespace Cafe.Infrastructure.Repositories.Commands
{
    public interface ICafeEmployeeCommandRepository : ICommandRepository<CafeEmployeeEntity>
    {
        Task AssignEmployeeToCafeAsync(Guid employeeId,Guid cafeId);
        Task UnassignEmployeeFromCafeAsync(Guid employeeId);
        Task UnassignListofEmployeesFromCafeAsync(Guid cafeId);
    }
}
