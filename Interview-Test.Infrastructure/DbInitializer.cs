using Interview_Test.Models;
using Microsoft.EntityFrameworkCore;

namespace Interview_Test.Infrastructure;

public static class DbInitializer
{
    public static void Seed(InterviewTestDbContext db)
    {
        if (db.UserTb.Any())
        {
            return;
        }

        var role1 = new RoleModel
        {
            RoleName = "pick operation",
            Permissions = new List<PermissionModel>
            {
                new() { Permission = "1-01-picking-info" },
                new() { Permission = "1-02-picking-start" },
                new() { Permission = "1-03-picking-confirm" },
                new() { Permission = "1-04-picking-report" },
                new() { Permission = "3-01-printing-label" }
            }
        };
        var role2 = new RoleModel
        {
            RoleName = "pack operation",
            Permissions = new List<PermissionModel>
            {
                new() { Permission = "1-04-picking-report" },
                new() { Permission = "2-01-packing-info" },
                new() { Permission = "2-02-packing-start" },
                new() { Permission = "2-03-packing-confirm" },
                new() { Permission = "2-04-packing-report" },
                new() { Permission = "3-01-printing-label" }
            }
        };
        var role3 = new RoleModel
        {
            RoleName = "document operation",
            Permissions = new List<PermissionModel>
            {
                new() { Permission = "1-04-picking-report" },
                new() { Permission = "2-04-packing-report" },
                new() { Permission = "3-01-printing-label" }
            }
        };
        db.RoleTb.AddRange(role1, role2, role3);
        db.SaveChanges();

        var user01 = new UserModel
        {
            Id = Guid.Parse("02CE43A4-A378-4B30-B52E-227EFA6B696E"),
            UserId = "user01",
            Username = "John.D.Smith",
            UserProfile = new UserProfileModel
            {
                FirstName = "John",
                LastName = "Smith",
                Age = null
            },
            UserRoleMappings = new List<UserRoleMappingModel>
            {
                new() { Role = role1 },
                new() { Role = role2 },
                new() { Role = role3 }
            }
        };
        var user02 = new UserModel
        {
            Id = Guid.Parse("F90810B6-E017-431A-9DAE-A4BA7F9BC865"),
            UserId = "user02",
            Username = "Bob.M.Jackson",
            UserProfile = new UserProfileModel
            {
                FirstName = "Bob",
                LastName = "Jackson",
                Age = 28
            },
            UserRoleMappings = new List<UserRoleMappingModel>
            {
                new() { Role = role3 }
            }
        };

        db.UserTb.AddRange(user01, user02);
        db.SaveChanges();
    }
}
