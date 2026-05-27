using System.Linq.Expressions;

namespace Interview_Test.Repositories.Generic;

// Generic Repository — ทำงานกับ entity ทุกตัวผ่าน type parameter T
// ไม่ต้องสร้าง UserRepository, RoleRepository แยกทีละตัว ลด boilerplate
public interface IAsyncRepository<T> where T : class
{
    // เปิด IQueryable ให้ Service เขียน LINQ ซับซ้อนเองได้
    IQueryable<T> Query();

    // ดึง list ตาม predicate + include แบบ Expression (top-level)
    Task<List<T>> ListAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

    // ดึงตัวแรกตาม predicate
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

    // path-based include — รองรับ nested เช่น "UserRoles.Role.RolePermissions.Permission"
    Task<List<T>> ListByPathAsync(Expression<Func<T, bool>> predicate, params string[] paths);
    Task<T?> FirstOrDefaultByPathAsync(Expression<Func<T, bool>> predicate, params string[] paths);

    Task AddAsync(T entity);
}
