using Cafe.Infrastructure.Repositories.Commands;
using Cafe.Infrastructure.Repositories.Queries;

namespace Cafe.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ICafeCommandRepository CafeCommand { get; }
        ICafeQueryRepository CafeQuery { get; }
        IEmployeeCommandRepository EmployeeCommand { get; }
        IEmployeeQueryRepository EmployeeQuery { get; }
        ICafeEmployeeCommandRepository CafeEmployeeCommand { get; }
        ICafeEmployeeQueryRepository CafeEmployeeQuery { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveChangesAsync();
    }
}
