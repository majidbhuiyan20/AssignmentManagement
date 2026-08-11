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
    if (id <= 0)
    {
        return BadRequest(new
        {
            message = "Invalid user id."
        });
    }

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
    if (id <= 0)
    {
        return BadRequest(new
        {
            message = "Invalid user id."
        });
    }

    UserResponse? user;

    try
    {
        user = await _userService.UpdateUserAsync(
            id,
            request);
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(new
        {
            message = ex.Message
        });
    }

    if (user == null)
    {
        return NotFound(new
        {
            message = "User not found."
        });
    }

    return Ok(user);
}
    [HttpDelete("{id}")]
public async Task<IActionResult> DeleteUser(int id)
{
    if (id <= 0)
    {
        return BadRequest(new
        {
            message = "Invalid user id."
        });
    }

    bool deleted = await _userService.DeleteUserAsync(id);

    if (!deleted)
    {
        return NotFound(new
        {
            message = "User not found."
        });
    }

    return Ok(new
    {
        message = "User deleted successfully."
    });
}
}
