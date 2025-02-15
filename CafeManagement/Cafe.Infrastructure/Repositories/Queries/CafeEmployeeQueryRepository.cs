using Cafe.Domain.Entities;
using Cafe.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Infrastructure.Repositories.Queries
{
    public class CafeEmployeeQueryRepository : ICafeEmployeeQueryRepository
    {
        private readonly CafeDbContext _context;

        public CafeEmployeeQueryRepository(CafeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CafeEmployeeEntity>> GetAllAsync()
        {
            return await _context.CafeEmployees.ToListAsync();
        }

        public async Task<CafeEmployeeEntity> GetByIdAsync(Guid id)
        {
            return await _context.CafeEmployees.FindAsync(id);
        }

        public async Task<CafeEmployeeEntity> GetCurrentEmploymentAsync(Guid employeeId)
        {
            return await _context.CafeEmployees
                .FirstOrDefaultAsync(ce => ce.EmployeeId == employeeId && ce.IsActive);
        }

        public async Task<IEnumerable<CafeEmployeeEntity>> GetCurrentEmployeesByCafeAsync(Guid cafeId)
        {
            return await _context.CafeEmployees
                .Where(ce => ce.CafeId == cafeId && ce.IsActive)
                .OrderByDescending(ce => ce.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CafeEmployeeEntity>> GetAllCafeEmployeeByEmployeeIdAsync(Guid employeeId)
        {
            return await _context.CafeEmployees
                .Where(ce => ce.EmployeeId == employeeId)
                .ToListAsync();
        }
    }
}
