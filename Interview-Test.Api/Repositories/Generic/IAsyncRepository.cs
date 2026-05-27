using System.Linq.Expressions;

namespace Interview_Test.Repositories.Generic;

public interface IAsyncRepository<T> where T : class
{
    IQueryable<T> Query();
    Task<List<T>> ListAllAsync(params Expression<Func<T, object>>[] includes);
    Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task AddAsync(T entity);
}
