using Interview_Test.Models;
using Microsoft.EntityFrameworkCore;

namespace Interview_Test.Infrastructure;

// seed ครั้งแรกตอน start app
public static class DbInitializer
{
    public static void Seed(InterviewTestDbContext db)
    {
        if (db.Users.Any())
        {
            return;
        }

        // 1) permissions master
        var pPickInfo = new PermissionModel { Permission = "1-01-picking-info" };
        var pPickStart = new PermissionModel { Permission = "1-02-picking-start" };
        var pPickConfirm = new PermissionModel { Permission = "1-03-picking-confirm" };
        var pPickReport = new PermissionModel { Permission = "1-04-picking-report" };
        var pPackInfo = new PermissionModel { Permission = "2-01-packing-info" };
        var pPackStart = new PermissionModel { Permission = "2-02-packing-start" };
        var pPackConfirm = new PermissionModel { Permission = "2-03-packing-confirm" };
        var pPackReport = new PermissionModel { Permission = "2-04-packing-report" };
        var pPrintLabel = new PermissionModel { Permission = "3-01-printing-label" };
        db.Permissions.AddRange(pPickInfo, pPickStart, pPickConfirm, pPickReport,
                                pPackInfo, pPackStart, pPackConfirm, pPackReport, pPrintLabel);
        db.SaveChanges();

        // 2) roles
        var role1 = new RoleModel { RoleName = "pick operation" };
        var role2 = new RoleModel { RoleName = "pack operation" };
        var role3 = new RoleModel { RoleName = "document operation" };
        db.Roles.AddRange(role1, role2, role3);
        db.SaveChanges();

        // 3) role-permissions
        db.RolePermissions.AddRange(
            // pick
            new RolePermissionModel { Role = role1, Permission = pPickInfo },
            new RolePermissionModel { Role = role1, Permission = pPickStart },
            new RolePermissionModel { Role = role1, Permission = pPickConfirm },
            new RolePermissionModel { Role = role1, Permission = pPickReport },
            new RolePermissionModel { Role = role1, Permission = pPrintLabel },
            // pack
            new RolePermissionModel { Role = role2, Permission = pPickReport },
            new RolePermissionModel { Role = role2, Permission = pPackInfo },
            new RolePermissionModel { Role = role2, Permission = pPackStart },
            new RolePermissionModel { Role = role2, Permission = pPackConfirm },
            new RolePermissionModel { Role = role2, Permission = pPackReport },
            new RolePermissionModel { Role = role2, Permission = pPrintLabel },
            // document
            new RolePermissionModel { Role = role3, Permission = pPickReport },
            new RolePermissionModel { Role = role3, Permission = pPackReport },
            new RolePermissionModel { Role = role3, Permission = pPrintLabel }
        );
        db.SaveChanges();

        // 4) users
        var user01 = new UserModel
        {
            Id = Guid.Parse("02CE43A4-A378-4B30-B52E-227EFA6B696E"),
            UserId = "user01",
            Username = "John.D.Smith",
            FirstName = "John",
            LastName = "Smith",
            Age = null
        };
        var user02 = new UserModel
        {
            Id = Guid.Parse("F90810B6-E017-431A-9DAE-A4BA7F9BC865"),
            UserId = "user02",
            Username = "Bob.M.Jackson",
            FirstName = "Bob",
            LastName = "Jackson",
            Age = 28
        };
        db.Users.AddRange(user01, user02);
        db.SaveChanges();

        // 5) user-roles
        db.UserRoles.AddRange(
            new UserRoleModel { User = user01, Role = role1 },
            new UserRoleModel { User = user01, Role = role2 },
            new UserRoleModel { User = user01, Role = role3 },
            new UserRoleModel { User = user02, Role = role3 }
        );
        db.SaveChanges();
    }
}
