namespace Cafe.Infrastructure.Repositories.Interfaces
{
    public interface IQueryRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
    }
}
