using Cafe.Domain.Entities;
using Cafe.Infrastructure.Context;
using Cafe.Infrastructure.Repositories.Queries;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Infrastructure.Repositories.Commands
{
    public class CafeEmployeeCommandRepository : ICafeEmployeeCommandRepository
    {
        private readonly CafeDbContext _context;
        private readonly ICafeEmployeeQueryRepository _queryRepository;

        public CafeEmployeeCommandRepository(CafeDbContext context, ICafeEmployeeQueryRepository queryRepository)
        {
            _context = context;
            _queryRepository = queryRepository;
        }

        public async Task<CafeEmployeeEntity> AddAsync(CafeEmployeeEntity entity)
        {
            await _context.CafeEmployees.AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(CafeEmployeeEntity entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.CafeEmployees.Attach(entity);
            }
            entry.State = EntityState.Modified;
        }

        public async Task AssignEmployeeToCafeAsync(Guid employeeId, Guid cafeId)
        {
            var currentAssignment = await _queryRepository.GetCurrentEmploymentAsync(employeeId);

            if (currentAssignment?.CafeId == cafeId)
                return;

            if (currentAssignment != null)
            {
                currentAssignment.Deactivate();
                await UpdateAsync(currentAssignment);
            }

            var newAssignment = new CafeEmployeeEntity
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                CafeId = cafeId,
                StartDate = DateTime.UtcNow,
                IsActive = true
            };
            await AddAsync(newAssignment);
        }

        public async Task UnassignEmployeeFromCafeAsync(Guid employeeId)
        {
            var currentAssignment = await _queryRepository.GetCurrentEmploymentAsync(employeeId);
            if (currentAssignment != null)
            {
                currentAssignment.Deactivate();
                await UpdateAsync(currentAssignment);
            }
        }

        public async Task UnassignListofEmployeesFromCafeAsync(Guid cafeId)
        {
            var currentAssignments = await _queryRepository.GetCurrentEmployeesByCafeAsync(cafeId);
            if (currentAssignments?.Any() == true)
            {
                foreach (var assignment in currentAssignments)
                {
                    assignment.Deactivate();
                    await UpdateAsync(assignment);
                }
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.CafeEmployees.AnyAsync(c => c.Id == id);
        }
    }
}
