using Interview_Test.Infrastructure;
using Interview_Test.Repositories.Generic;

namespace Interview_Test.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly InterviewTestDbContext _db;
    private readonly Dictionary<Type, object> _repos = new();

    public UnitOfWork(InterviewTestDbContext db)
    {
        _db = db;
    }

    public IAsyncRepository<T> AsyncRepository<T>() where T : class
    {
        if (_repos.TryGetValue(typeof(T), out var existing))
        {
            return (IAsyncRepository<T>)existing;
        }
        var repo = new AsyncRepository<T>(_db);
        _repos[typeof(T)] = repo;
        return repo;
    }

    public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
}
