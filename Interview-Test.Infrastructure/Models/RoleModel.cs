using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview_Test.Models;

[Table("Roles")]
public class RoleModel
{
    [Required]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RoleId { get; set; }

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string RoleName { get; set; }

    // junctions
    public ICollection<UserRoleModel> UserRoles { get; set; }
    public ICollection<RolePermissionModel> RolePermissions { get; set; }
}
