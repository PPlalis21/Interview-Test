using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview_Test.Models;

// junction: Role ↔ Permission
[Table("RolePermissions")]
public class RolePermissionModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid RolePermissionId { get; set; }

    public int RoleId { get; set; }
    public long PermissionId { get; set; }

    public RoleModel Role { get; set; }
    public PermissionModel Permission { get; set; }
}
