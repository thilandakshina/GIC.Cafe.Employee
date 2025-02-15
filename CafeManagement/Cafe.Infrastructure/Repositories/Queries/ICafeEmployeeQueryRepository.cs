using Cafe.Domain.Entities;
using Cafe.Infrastructure.Repositories.Interfaces;

namespace Cafe.Infrastructure.Repositories.Queries
{
    public interface ICafeEmployeeQueryRepository : IQueryRepository<CafeEmployeeEntity>
    {
        Task<CafeEmployeeEntity> GetCurrentEmploymentAsync(Guid employeeId);
        Task<IEnumerable<CafeEmployeeEntity>> GetCurrentEmployeesByCafeAsync(Guid cafeId);
        Task<IEnumerable<CafeEmployeeEntity>> GetAllCafeEmployeeByEmployeeIdAsync(Guid employeeId);
    }
}
