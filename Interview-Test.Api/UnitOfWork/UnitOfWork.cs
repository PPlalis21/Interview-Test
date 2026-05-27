using Interview_Test.Infrastructure;
using Interview_Test.Repositories.Generic;

namespace Interview_Test.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly InterviewTestDbContext _db;
    // cache repo ตาม entity type — สร้างครั้งเดียวต่อ scope (1 ต่อ request)
    private readonly Dictionary<Type, object> _repos = new();

    public UnitOfWork(InterviewTestDbContext db)
    {
        _db = db;
    }

    // ขอ Generic Repository — ถ้ายังไม่มีก็สร้างใหม่แล้ว cache ไว้
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

    // commit ทุก change ใน DbContext เป็น 1 transaction (atomic — สำเร็จด้วยกัน/พังด้วยกัน)
    public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
}
