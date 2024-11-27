using Cafe.Domain.Entities;
using Cafe.Domain.Models;
using Cafe.Infrastructure.Repositories.Interfaces;

namespace Cafe.Infrastructure.Repositories.Queries
{
    public interface ICafeQueryRepository : IQueryRepository<CafeEntity>
    {
        Task<CafeWithEmployees> GetCafeByIdAsync(Guid? cafeId);
        Task<IEnumerable<CafeWithEmployees>> GetAllWithEmployeeCountAsync(string location = null);

    }
}
