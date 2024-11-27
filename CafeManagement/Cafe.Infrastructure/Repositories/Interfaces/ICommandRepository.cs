namespace Cafe.Infrastructure.Repositories.Interfaces
{
    public interface ICommandRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task<bool> ExistsAsync(Guid id);

    }
}
