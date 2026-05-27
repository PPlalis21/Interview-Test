using Interview_Test.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Interview_Test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<RoleController> _logger;

    public RoleController(IRoleRepository roleRepository, ILogger<RoleController> logger)
    {
        _roleRepository = roleRepository;
        _logger = logger;
    }

    // GET /api/role/GetRoles — master data
    [HttpGet("GetRoles")]
    public ActionResult GetRoles()
    {
        try
        {
            return Ok(_roleRepository.GetRoles());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get roles");
            return StatusCode(500, new { message = "Failed to get roles", error = ex.Message });
        }
    }
}
