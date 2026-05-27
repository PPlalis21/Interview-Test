using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview_Test.Models;

[Table("Users")]
public class UserModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string UserId { get; set; }

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string Username { get; set; }

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string FirstName { get; set; }

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string LastName { get; set; }

    public int? Age { get; set; }

    // → UserRoles (many-to-many กับ Role)
    public ICollection<UserRoleModel> UserRoles { get; set; }
}
