using Cafe.Domain.Entities;
using Cafe.Domain.Models;
using Cafe.Infrastructure.Repositories.Interfaces;

namespace Cafe.Infrastructure.Repositories.Queries
{
    public interface IEmployeeQueryRepository : IQueryRepository<EmployeeEntity>
    {
        Task<IEnumerable<EmployeeEntity>> GetByCafeIdAsync(Guid cafeId);
        Task<bool> IsEmployeeAssignedToCafe(Guid employeeId);
        Task<IEnumerable<EmployeeWithDetails>> GetAllEmployeesWithDetailsAsync();
        Task<EmployeeWithDetails> GetEmployeeWithDetailsAsync(Guid employeeId);
    }
}
