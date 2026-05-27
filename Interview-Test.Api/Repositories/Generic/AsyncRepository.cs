using System.Linq.Expressions;
using Interview_Test.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Interview_Test.Repositories.Generic;

public class AsyncRepository<T> : IAsyncRepository<T> where T : class
{
    private readonly InterviewTestDbContext _db;
    private readonly DbSet<T> _set;

    public AsyncRepository(InterviewTestDbContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public IQueryable<T> Query() => _set.AsQueryable();

    public async Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> q = _set;
        foreach (var inc in includes) q = q.Include(inc);
        return await q.Where(predicate).ToListAsync();
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> q = _set;
        foreach (var inc in includes) q = q.Include(inc);
        return await q.FirstOrDefaultAsync(predicate);
    }

    public async Task<List<T>> ListByPathAsync(Expression<Func<T, bool>> predicate, params string[] paths)
    {
        IQueryable<T> q = _set;
        foreach (var p in paths) q = q.Include(p);
        return await q.Where(predicate).ToListAsync();
    }

    public async Task<T?> FirstOrDefaultByPathAsync(Expression<Func<T, bool>> predicate, params string[] paths)
    {
        IQueryable<T> q = _set;
        foreach (var p in paths) q = q.Include(p);
        return await q.FirstOrDefaultAsync(predicate);
    }

    public async Task AddAsync(T entity)
    {
        await _set.AddAsync(entity);
    }
}
