namespace Interview_Test.Dtos;

public class UserListDto
{
    public Guid id { get; set; }
    public string userId { get; set; }
    public string username { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public int? age { get; set; }
    public int rolesCount { get; set; }
    public int permissionsCount { get; set; }
}

public class RoleDto
{
    public int roleId { get; set; }
    public string roleName { get; set; }
}

public class UserDetailDto
{
    public Guid id { get; set; }
    public string userId { get; set; }
    public string username { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public int? age { get; set; }
    public List<RoleDto> roles { get; set; }
    public List<string> permissions { get; set; }
}
