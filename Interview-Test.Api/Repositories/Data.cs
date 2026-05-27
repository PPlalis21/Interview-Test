using Interview_Test.Models;

namespace Interview_Test.Repositories;

// ตัวอย่าง payload สำหรับทดสอบ POST /CreateUser
public static class Data
{
    public static List<UserModel> Users =>
    [
        new UserModel
        {
            Id = Guid.Parse("F90810B6-E017-431A-9DAE-A4BA7F9BC865"),
            UserId = "user02",
            Username = "Bob.M.Jackson",
            FirstName = "Bob",
            LastName = "Jackson",
            Age = 28,
            UserRoles = new List<UserRoleModel>
            {
                new() { Role = new RoleModel { RoleId = 3 } }
            }
        },
        new UserModel
        {
            Id = Guid.Parse("02CE43A4-A378-4B30-B52E-227EFA6B696E"),
            UserId = "user01",
            Username = "John.D.Smith",
            FirstName = "John",
            LastName = "Smith",
            Age = null,
            UserRoles = new List<UserRoleModel>
            {
                new() { Role = new RoleModel { RoleId = 1 } },
                new() { Role = new RoleModel { RoleId = 2 } },
                new() { Role = new RoleModel { RoleId = 3 } }
            }
        }
    ];
}
