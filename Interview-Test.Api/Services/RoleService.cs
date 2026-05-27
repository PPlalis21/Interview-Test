using Interview_Test.Common;
using Interview_Test.Dtos;
using Interview_Test.Models;
using Interview_Test.UnitOfWork;

namespace Interview_Test.Services;

public class RoleService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse<List<RoleDto>>> GetRoles()
    {
        ServiceResponse<List<RoleDto>> response = new ServiceResponse<List<RoleDto>>();

        try
        {
            var roleRepo = _unitOfWork.AsyncRepository<RoleModel>();

            var roles = await roleRepo.ListAllAsync();

            var result = (from r in roles
                          orderby r.RoleId ascending
                          select new RoleDto
                          {
                              roleId = r.RoleId,
                              roleName = r.RoleName
                          }).ToList();

            response.Data = result;
            response.Success = true;
            response.Message = "OK";
        }
        catch (Exception ex)
        {
            response.Data = new List<RoleDto>();
            response.Success = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
