using System.Linq.Expressions;

namespace Interview_Test.Repositories.Generic;

public interface IAsyncRepository<T> where T : class
{
    IQueryable<T> Query();
    Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task<List<T>> ListByPathAsync(Expression<Func<T, bool>> predicate, params string[] paths);
    Task<T?> FirstOrDefaultByPathAsync(Expression<Func<T, bool>> predicate, params string[] paths);
    Task AddAsync(T entity);
}
