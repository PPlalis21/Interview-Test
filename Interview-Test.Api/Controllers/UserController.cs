using Interview_Test.Models;
using Interview_Test.Services;
using Microsoft.AspNetCore.Mvc;

namespace Interview_Test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _userService.GetUsers();
        return Ok(result);
    }

    [HttpGet("GetUserById/{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var result = await _userService.GetUserById(id);
        return Ok(result);
    }

    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser(UserModel user)
    {
        var result = await _userService.CreateUser(user);
        return Ok(result);
    }
}
