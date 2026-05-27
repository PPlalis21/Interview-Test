using Interview_Test.Repositories.Generic;

namespace Interview_Test.UnitOfWork;

public interface IUnitOfWork
{
    IAsyncRepository<T> AsyncRepository<T>() where T : class;
    Task<int> SaveChangesAsync();
}
