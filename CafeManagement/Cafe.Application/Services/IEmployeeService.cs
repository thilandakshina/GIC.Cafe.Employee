using Cafe.Domain.Entities;
using Cafe.Domain.Models;

namespace Cafe.Application.Services
{
    public interface IEmployeeService
    {
        Task<Guid> CreateAsync(EmployeeEntity employee, Guid? cafeId);
        Task<bool> UpdateAsync(EmployeeEntity employee, Guid? cafeId);
        Task<bool> DeleteAsync(Guid id);
        Task<EmployeeWithDetails> GetByIdAsync(Guid id);
        Task<IEnumerable<EmployeeWithDetails>> GetAllAsync(Guid? cafeId);
    }
}
