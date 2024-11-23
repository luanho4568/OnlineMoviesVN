using System.Linq.Expressions;

namespace OnlineMoviesVN.DAL.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<IEnumerable<IGrouping<TKey, T>>> GetGroupedAsync<TKey>(Expression<Func<T, bool>>? filter, Expression<Func<T, TKey>> groupBy);

        Task AddAsync(T entity);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);
    }
}
