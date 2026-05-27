using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview_Test.Models;

// junction: User ↔ Role
[Table("UserRoles")]
public class UserRoleModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid UserRoleId { get; set; }

    public Guid UserId { get; set; }
    public int RoleId { get; set; }

    public UserModel User { get; set; }
    public RoleModel Role { get; set; }
}
