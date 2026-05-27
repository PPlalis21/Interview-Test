using Interview_Test.Infrastructure;
using Interview_Test.Models;
using Interview_Test.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Interview_Test.Repositories;

public class UserRepository : IUserRepository
{
    private readonly InterviewTestDbContext _db;

    public UserRepository(InterviewTestDbContext db)
    {
        _db = db;
    }

    // หา user ด้วย Guid หรือ UserId
    public dynamic GetUserById(string id)
    {
        Guid.TryParse(id, out var guid);

        var result = (from u in _db.Users
                      where u.Id == guid || u.UserId == id
                      select new
                      {
                          id = u.Id,
                          userId = u.UserId,
                          username = u.Username,
                          firstName = u.FirstName,
                          lastName = u.LastName,
                          age = u.Age,
                          // roles ของ user
                          roles = u.UserRoles
                                   .OrderBy(ur => ur.Role.RoleId)
                                   .Select(ur => new
                                   {
                                       roleId = ur.Role.RoleId,
                                       roleName = ur.Role.RoleName
                                   })
                                   .ToList(),
                          // permissions รวมจากทุก role + distinct
                          permissions = u.UserRoles
                                         .SelectMany(ur => ur.Role.RolePermissions
                                                                  .Select(rp => rp.Permission.Permission))
                                         .Distinct()
                                         .OrderBy(p => p)
                                         .ToList()
                      })
                     .FirstOrDefault();

        return result!;
    }

    // insert user + role mappings
    public int CreateUser(UserModel user)
    {
        // ผูก Role ที่มีอยู่แล้วใน DB กัน EF insert ซ้ำ
        if (user.UserRoles != null)
        {
            foreach (var ur in user.UserRoles)
            {
                if (ur.Role != null && ur.Role.RoleId > 0)
                {
                    var existing = _db.Roles.Find(ur.Role.RoleId);
                    if (existing != null)
                    {
                        ur.Role = existing;
                    }
                }
            }
        }
        _db.Users.Add(user);
        return _db.SaveChanges();
    }

    // list สำหรับหน้า users — ส่ง count แทนข้อมูลเต็ม
    public dynamic GetUsers()
    {
        var result = (from u in _db.Users
                      orderby u.UserId
                      select new
                      {
                          id = u.Id,
                          userId = u.UserId,
                          username = u.Username,
                          firstName = u.FirstName,
                          lastName = u.LastName,
                          age = u.Age,
                          rolesCount = u.UserRoles.Count(),
                          permissionsCount = u.UserRoles
                              .SelectMany(ur => ur.Role.RolePermissions
                                                       .Select(rp => rp.Permission.Permission))
                              .Distinct().Count()
                      }).ToList();
        return result;
    }
}
