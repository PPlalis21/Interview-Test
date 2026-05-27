using Interview_Test.Infrastructure;
using Interview_Test.Repositories.Interfaces;

namespace Interview_Test.Repositories;

// Role master-data
public class RoleRepository : IRoleRepository
{
    private readonly InterviewTestDbContext _db;

    public RoleRepository(InterviewTestDbContext db)
    {
        _db = db;
    }

    public dynamic GetRoles()
    {
        var result = (from r in _db.Roles
                      orderby r.RoleId
                      select new
                      {
                          roleId = r.RoleId,
                          roleName = r.RoleName
                      }).ToList();
        return result;
    }
}
