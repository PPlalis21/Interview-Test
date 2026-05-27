using Interview_Test.Repositories.Generic;

namespace Interview_Test.UnitOfWork;

// Unit of Work pattern — รวมหลาย Repository ภายใต้ DbContext เดียวกัน
// AsyncRepository<T>() → ขอ Generic Repository ตาม entity type
// SaveChangesAsync() → commit การเปลี่ยนแปลงทั้งหมดเป็น 1 transaction (atomic)
public interface IUnitOfWork
{
    IAsyncRepository<T> AsyncRepository<T>() where T : class;
    Task<int> SaveChangesAsync();
}
