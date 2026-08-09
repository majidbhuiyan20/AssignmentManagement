using AssignmentManagement.DTOs.Users;
using AssignmentManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentManagement.Controllers;

[ApiController]
[Route("api/user")]
[Authorize(Roles = "Admin")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();

        return Ok(users);
    }


    [HttpGet("{id}")]
public async Task<IActionResult> GetUserById(int id)
{
    var user = await _userService.GetUserByIdAsync(id);

    if (user == null)
    {
        return NotFound(new
        {
            message = "User not found."
        });
    }

    return Ok(user);
}

[HttpPut("{id}")]
public async Task<IActionResult> UpdateUser(
    int id,
    UpdateUserRequest request)
{
    var user = await _userService.UpdateUserAsync(
        id,
        request);

    if (user == null)
    {
        return NotFound(new
        {
            message = "User not found."
        });
    }

    return Ok(user);
}
}