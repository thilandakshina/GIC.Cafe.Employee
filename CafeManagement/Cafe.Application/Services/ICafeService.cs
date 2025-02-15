using Cafe.Domain.Entities;
using Cafe.Domain.Models;

namespace Cafe.Application.Services
{
    public interface ICafeService
    {
        Task<Guid> CreateAsync(CafeEntity cafe);
        Task<bool> UpdateAsync(CafeEntity cafe);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<CafeWithEmployees>> GetAllAsync(string location = null);
        Task<CafeWithEmployees> GetByIdAsync(Guid id);
    }
}
