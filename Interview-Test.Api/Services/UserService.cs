using Interview_Test.Common;
using Interview_Test.Dtos;
using Interview_Test.Models;
using Interview_Test.UnitOfWork;

namespace Interview_Test.Services;

public class UserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // list ผู้ใช้ + count
    public async Task<ServiceResponse<List<UserListDto>>> GetUsers()
    {
        ServiceResponse<List<UserListDto>> response = new ServiceResponse<List<UserListDto>>();

        try
        {
            var userRepo = _unitOfWork.AsyncRepository<UserModel>();

            var users = await userRepo.ListByPathAsync(x => true, "UserRoles.Role.RolePermissions.Permission");

            var result = (from u in users
                          orderby u.UserId ascending
                          select new UserListDto
                          {
                              id = u.Id,
                              userId = u.UserId,
                              username = u.Username,
                              firstName = u.FirstName,
                              lastName = u.LastName,
                              age = u.Age,
                              rolesCount = u.UserRoles.Count(),
                              permissionsCount = u.UserRoles
                                  .SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Permission))
                                  .Distinct().Count()
                          }).ToList();

            response.Data = result;
            response.Success = true;
            response.Message = "OK";
        }
        catch (Exception ex)
        {
            response.Data = new List<UserListDto>();
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    // หา user ด้วย Guid หรือ UserId
    public async Task<ServiceResponse<UserDetailDto?>> GetUserById(string id)
    {
        ServiceResponse<UserDetailDto?> response = new ServiceResponse<UserDetailDto?>();

        try
        {
            Guid.TryParse(id, out var guid);

            var userRepo = _unitOfWork.AsyncRepository<UserModel>();

            var users = await userRepo.ListByPathAsync(x => x.Id == guid || x.UserId == id,
                                                       "UserRoles.Role.RolePermissions.Permission");

            var result = (from u in users
                          select new UserDetailDto
                          {
                              id = u.Id,
                              userId = u.UserId,
                              username = u.Username,
                              firstName = u.FirstName,
                              lastName = u.LastName,
                              age = u.Age,
                              roles = (from ur in u.UserRoles
                                       orderby ur.Role.RoleId
                                       select new RoleDto
                                       {
                                           roleId = ur.Role.RoleId,
                                           roleName = ur.Role.RoleName
                                       }).ToList(),
                              permissions = u.UserRoles
                                             .SelectMany(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Permission))
                                             .Distinct()
                                             .OrderBy(p => p)
                                             .ToList()
                          }).FirstOrDefault();

            response.Data = result;
            response.Success = result != null;
            response.Message = result != null ? "OK" : "User not found";
        }
        catch (Exception ex)
        {
            response.Data = null;
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }

    // insert user + role mappings
    public async Task<ServiceResponse<int>> CreateUser(UserModel user)
    {
        ServiceResponse<int> response = new ServiceResponse<int>();

        try
        {
            var userRepo = _unitOfWork.AsyncRepository<UserModel>();
            var roleRepo = _unitOfWork.AsyncRepository<RoleModel>();

            // กัน UserId ซ้ำ — ตรวจที่ app-level ก่อนยิง DB (DB ก็มี unique index กันอีกชั้น)
            var duplicate = await userRepo.FirstOrDefaultAsync(u => u.UserId == user.UserId);
            if (duplicate != null)
            {
                response.Data = 0;
                response.Success = false;
                response.Message = $"UserId '{user.UserId}' already exists";
                return response;
            }

            // ผูก Role เดิมที่มีใน DB กัน EF insert ซ้ำ
            if (user.UserRoles != null)
            {
                foreach (var ur in user.UserRoles)
                {
                    if (ur.Role != null && ur.Role.RoleId > 0)
                    {
                        var existing = await roleRepo.FirstOrDefaultAsync(r => r.RoleId == ur.Role.RoleId);
                        if (existing != null) ur.Role = existing;
                    }
                }
            }

            await userRepo.AddAsync(user);
            var affectedRows = await _unitOfWork.SaveChangesAsync();

            response.Data = affectedRows;
            response.Success = true;
            response.Message = "OK";
        }
        catch (Exception ex)
        {
            response.Data = 0;
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
