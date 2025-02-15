using Cafe.Domain.Entities;
using Cafe.Infrastructure.Repositories.Interfaces;

namespace Cafe.Infrastructure.Repositories.Commands
{
    public interface ICafeCommandRepository : ICommandRepository<CafeEntity>
    {
    }
}
