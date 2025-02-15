using Cafe.Domain.Entities;
using Cafe.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Infrastructure.Repositories.Commands
{
    public class CafeCommandRepository : ICafeCommandRepository
    {
        private readonly CafeDbContext _context;

        public CafeCommandRepository(CafeDbContext context)
        {
            _context = context;
        }

        public async Task<CafeEntity> AddAsync(CafeEntity entity)
        {
            await _context.Cafes.AddAsync(entity);
            return entity;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Cafes.AnyAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(CafeEntity entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _context.Cafes.Attach(entity);
            }
            entry.State = EntityState.Modified;
        }
    }
}
