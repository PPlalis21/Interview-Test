using Interview_Test.Models;
using Interview_Test.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Interview_Test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserController> _logger;

    // DI
    public UserController(IUserRepository userRepository, ILogger<UserController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    // GET /api/user/GetUsers
    [HttpGet("GetUsers")]
    public ActionResult GetUsers()
    {
        try
        {
            return Ok(_userRepository.GetUsers());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get users");
            return StatusCode(500, new { message = "Failed to get users", error = ex.Message });
        }
    }

    // GET /api/user/GetUserById/{id}
    [HttpGet("GetUserById/{id}")]
    public ActionResult GetUserById(string id)
    {
        try
        {
            var result = _userRepository.GetUserById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get user by id {Id}", id);
            return StatusCode(500, new { message = "Failed to get user", error = ex.Message });
        }
    }

    // POST /api/user/CreateUser
    [HttpPost("CreateUser")]
    public ActionResult CreateUser(UserModel user)
    {
        try
        {
            var affectedRows = _userRepository.CreateUser(user);
            return Ok(new { affectedRows });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create user");
            return StatusCode(500, new { message = "Failed to create user", error = ex.Message });
        }
    }
}
