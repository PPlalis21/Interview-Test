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

    public dynamic GetUserById(string id)
    {
        Guid.TryParse(id, out var guid);
        var result = (from u in _db.UserTb
                      where u.Id == guid || u.UserId == id
                      select new
                      {
                          id = u.Id,
                          userId = u.UserId,
                          username = u.Username,
                          firstName = u.UserProfile.FirstName,
                          lastName = u.UserProfile.LastName,
                          age = u.UserProfile.Age,
                          roles = u.UserRoleMappings
                                   .OrderBy(urm => urm.Role.RoleId)
                                   .Select(urm => new
                                   {
                                       roleId = urm.Role.RoleId,
                                       roleName = urm.Role.RoleName
                                   })
                                   .ToList(),
                          permissions = u.UserRoleMappings
                                         .SelectMany(urm => urm.Role.Permissions.Select(p => p.Permission))
                                         .Distinct()
                                         .OrderBy(p => p)
                                         .ToList()
                      })
                     .FirstOrDefault();

        return result!;
    }

    public int CreateUser(UserModel user)
    {
        if (user.UserRoleMappings != null)
        {
            foreach (var mapping in user.UserRoleMappings)
            {
                if (mapping.Role != null && mapping.Role.RoleId > 0)
                {
                    var existing = _db.RoleTb.Find(mapping.Role.RoleId);
                    if (existing != null)
                    {
                        mapping.Role = existing;
                    }
                }
            }
        }
        _db.UserTb.Add(user);
        return _db.SaveChanges();
    }

    public dynamic GetUsers()
    {
        var result = (from u in _db.UserTb
                      orderby u.UserId
                      select new
                      {
                          id = u.Id,
                          userId = u.UserId,
                          username = u.Username,
                          firstName = u.UserProfile.FirstName,
                          lastName = u.UserProfile.LastName,
                          age = u.UserProfile.Age,
                          rolesCount = u.UserRoleMappings.Count(),
                          permissionsCount = u.UserRoleMappings
                              .SelectMany(urm => urm.Role.Permissions.Select(p => p.Permission))
                              .Distinct().Count()
                      }).ToList();
        return result;
    }
}
