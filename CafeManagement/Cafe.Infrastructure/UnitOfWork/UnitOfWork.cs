using Cafe.Infrastructure.Context;
using Cafe.Infrastructure.Repositories.Commands;
using Cafe.Infrastructure.Repositories.Queries;
using Microsoft.EntityFrameworkCore.Storage;

namespace Cafe.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CafeDbContext _context;
        private IDbContextTransaction _transaction;

        public ICafeCommandRepository CafeCommand { get; }
        public ICafeQueryRepository CafeQuery { get; }
        public IEmployeeCommandRepository EmployeeCommand { get; }
        public IEmployeeQueryRepository EmployeeQuery { get; }
        public ICafeEmployeeCommandRepository CafeEmployeeCommand { get; }
        public ICafeEmployeeQueryRepository CafeEmployeeQuery { get; }

        public UnitOfWork(
            CafeDbContext context,
            ICafeCommandRepository cafeCommand,
            ICafeQueryRepository cafeQuery,
            IEmployeeCommandRepository employeeCommand,
            IEmployeeQueryRepository employeeQuery,
            ICafeEmployeeCommandRepository cafeEmployeeCommand,
            ICafeEmployeeQueryRepository cafeEmployeeQuery)
        {
            _context = context;
            CafeCommand = cafeCommand;
            CafeQuery = cafeQuery;
            EmployeeCommand = employeeCommand;
            EmployeeQuery = employeeQuery;
            CafeEmployeeCommand = cafeEmployeeCommand;
            CafeEmployeeQuery = cafeEmployeeQuery;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch
            {
                await _transaction.RollbackAsync();
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}