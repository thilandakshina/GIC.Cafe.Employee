using Cafe.Domain.Entities;
using Cafe.Domain.Models;
using Cafe.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Infrastructure.Repositories.Queries
{
    public class EmployeeQueryRepository : IEmployeeQueryRepository
    {
        private readonly CafeDbContext _context;

        public EmployeeQueryRepository(CafeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeEntity>> GetAllAsync()
        {
            return await _context.Employees
                .Where(ce => ce.IsActive)
                .ToListAsync();
        }

        public async Task<EmployeeEntity> GetByIdAsync(Guid id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task<IEnumerable<EmployeeEntity>> GetByCafeIdAsync(Guid cafeId)
        {
            var activeAssignments = await _context.CafeEmployees
                .Where(ce => ce.CafeId == cafeId && ce.IsActive)
                .OrderByDescending(ce => ce.StartDate)
                .ToListAsync();

            var employeeIds = activeAssignments.Select(a => a.EmployeeId);

            return await _context.Employees
                .Where(e => employeeIds.Contains(e.Id))
                .ToListAsync();
        }

        public async Task<bool> IsEmployeeAssignedToCafe(Guid employeeId)
        {
            return await _context.CafeEmployees
                .AnyAsync(ce => ce.EmployeeId == employeeId && ce.IsActive);
        }

        public async Task<IEnumerable<EmployeeWithDetails>>
               GetAllEmployeesWithDetailsAsync()
        {
            var results = await _context.Employees
                .AsNoTracking()
                .Where(c => c.IsActive)
                .Select(e => new
                {
                    Employee = e,
                    AssignedCafe = e.CafeEmployees
                        .Where(ce => ce.IsActive)
                        .Select(ce => new CafeEntity
                        {
                            Id = ce.CafeEntity.Id,
                            Name = ce.CafeEntity.Name,
                            Location = ce.CafeEntity.Location,
                        })
                        .FirstOrDefault(),
                    CafeEmployments = e.CafeEmployees
                    .Select(ce => new CafeEmployeeEntity
                    {
                        Id = ce.Id,
                        CafeId = ce.CafeId,
                        StartDate = ce.StartDate,
                        EndDate = ce.EndDate,
                        IsActive = ce.IsActive,
                    })
                    .OrderByDescending(ce => ce.StartDate)
                    .ToList()
                }).ToListAsync();


                return results.Select(r => new EmployeeWithDetails(
               r.Employee,
               r.AssignedCafe,
               r.CafeEmployments
            ));
        }

        public async Task<EmployeeWithDetails> GetEmployeeWithDetailsAsync(Guid employeeId)
        {
            var result = await _context.Employees
                .AsNoTracking()
                .Where(e => e.Id == employeeId)
                .Select(e => new
                {
                    Employee = e,
                    AssignedCafe = e.CafeEmployees
                        .Where(ce => ce.IsActive)
                        .Select(ce => new CafeEntity
                        {
                            Id = ce.CafeEntity.Id,
                            Name = ce.CafeEntity.Name,
                            Location = ce.CafeEntity.Location,
                        })
                        .FirstOrDefault(),
                    CafeEmployments = e.CafeEmployees
                    .Select(ce => new CafeEmployeeEntity
                    {
                        Id = ce.Id,
                        CafeId = ce.CafeId,
                        StartDate = ce.StartDate,
                        EndDate = ce.EndDate,
                        IsActive = ce.IsActive,
                    })
                    .OrderByDescending(ce => ce.StartDate)
                    .ToList()
                })
                .FirstOrDefaultAsync();

            return result == null ?
           new EmployeeWithDetails(null, null, new List<CafeEmployeeEntity>()) :
           new EmployeeWithDetails(result.Employee, result.AssignedCafe, result.CafeEmployments);
        }
    }
}