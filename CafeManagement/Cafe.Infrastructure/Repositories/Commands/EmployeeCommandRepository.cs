using Cafe.Domain.Entities;
using Cafe.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Infrastructure.Repositories.Commands
{
    public class EmployeeCommandRepository : IEmployeeCommandRepository
    {
        private readonly CafeDbContext _context;
        private readonly ICafeEmployeeCommandRepository _cafeEmployeeCommandRepository;

        public EmployeeCommandRepository(
            CafeDbContext context,
            ICafeEmployeeCommandRepository cafeEmployeeCommandRepository)
        {
            _context = context;
            _cafeEmployeeCommandRepository = cafeEmployeeCommandRepository;
        }

        public async Task<EmployeeEntity> AddAsync(EmployeeEntity entity)
        {
            entity.IsActive = true;
            await _context.Employees.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(EmployeeEntity entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Employees.Attach(entity);
            }
            entry.State = EntityState.Modified;
        }

        public async Task DeactivateEmployeeAsync(Guid employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee != null)
            {
                employee.IsActive = false;
                await UpdateAsync(employee);

                // Unassign from current cafe if assigned
                await _cafeEmployeeCommandRepository.UnassignEmployeeFromCafeAsync(employeeId);
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Employees.AnyAsync(c => c.Id == id);
        }
    }
}
