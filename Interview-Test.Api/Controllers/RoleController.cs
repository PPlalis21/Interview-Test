using Interview_Test.Services;
using Microsoft.AspNetCore.Mvc;

namespace Interview_Test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly RoleService _roleService;

    public RoleController(RoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet("GetRoles")]
    public async Task<IActionResult> GetRoles()
    {
        var result = await _roleService.GetRoles();
        return Ok(result);
    }
}
