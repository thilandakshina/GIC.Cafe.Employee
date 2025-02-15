using Cafe.Domain.Entities;
using Cafe.Domain.Models;
using Cafe.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Infrastructure.Repositories.Queries
{
    public class CafeQueryRepository : ICafeQueryRepository
    {
        private readonly CafeDbContext _context;

        public CafeQueryRepository(CafeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CafeEntity>> GetAllAsync()
        {
            return await _context.Cafes.Where(ce => ce.IsActive).ToListAsync();
        }

        public async Task<CafeEntity> GetByIdAsync(Guid id)
        {
            return await _context.Cafes.FindAsync(id);
        }

        public async Task<CafeWithEmployees> GetCafeByIdAsync(Guid? cafeId)
        {
            var result = await _context.Cafes
                 .AsNoTracking()
                 .Where(c => c.Id == cafeId)
                 .Select(e => new
                 {
                     Cafe = e,
                     CafeEmployments = e.CafeEmployees
                     .Where(ce => ce.IsActive)
                     .Select(ce => new CafeEmployeeEntity
                     {
                         Id = ce.Id,
                         CafeId = ce.CafeId,
                         EmployeeId = ce.EmployeeId,
                         StartDate = ce.StartDate,
                         EndDate = ce.EndDate,
                         IsActive = ce.IsActive,
                     })
                     .OrderByDescending(ce => ce.StartDate)
                     .ToList()
                 }).FirstOrDefaultAsync();

            return result == null
                ? new CafeWithEmployees(null, new List<CafeEmployeeEntity>())
                : new CafeWithEmployees(result.Cafe, result.CafeEmployments);
        }

        public async Task<IEnumerable<CafeWithEmployees>> GetAllWithEmployeeCountAsync(string location)
        {
            var results = await _context.Cafes
                 .AsNoTracking()
                 .Where(c => c.IsActive && ( string.IsNullOrEmpty(location) ||
                    c.Location.ToLower().Contains(location.ToLower())))
                 .Select(e => new
                 {
                     Cafes = e,
                     CafeEmployments = e.CafeEmployees
                     .Where(ce => ce.IsActive)
                     .Select(ce => new CafeEmployeeEntity
                     {
                         Id = ce.Id,
                         CafeId = ce.CafeId,
                         EmployeeId = ce.EmployeeId,
                         StartDate = ce.StartDate,
                         EndDate = ce.EndDate,
                         IsActive = ce.IsActive,
                     })
                     .OrderByDescending(ce => ce.StartDate)
                     .ToList()
                 }).ToListAsync();

            return results.Select(r => new CafeWithEmployees(r.Cafes, r.CafeEmployments));
        }
    }
}
