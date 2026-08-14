using AssignmentManagement.DTOs.Assignments;
using AssignmentManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AssignmentManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AssignmentController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentController(
        IAssignmentService assignmentService)
    {
        _assignmentService = assignmentService;
    }

    [HttpPost]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Create(
        CreateAssignmentRequest request)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized(new
            {
                success = false,
                message = "User identity is invalid."
            });
        }

        AssignmentResponse result;

        try
        {
            result =
                await _assignmentService.CreateAssignmentAsync(
                    userId,
                    request);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                success = false,
                message = ex.Message
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                success = false,
                message = "Invalid assignment id."
            });
        }

        var result =
            await _assignmentService.GetAssignmentByIdAsync(id);

        if (result == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Assignment not found."
            });
        }

        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> GetAll()
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized(new
            {
                success = false,
                message = "User identity is invalid."
            });
        }

        var result =
            await _assignmentService.GetAllAssignmentsAsync(
                userId);

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Update(
        int id,
        UpdateAssignmentRequest request)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                success = false,
                message = "Invalid assignment id."
            });
        }

        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized(new
            {
                success = false,
                message = "User identity is invalid."
            });
        }

        AssignmentResponse? result;

        try
        {
            result =
                await _assignmentService.UpdateAssignmentAsync(
                    userId,
                    id,
                    request);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }

        if (result == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Assignment not found."
            });
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                success = false,
                message = "Invalid assignment id."
            });
        }

        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized(new
            {
                success = false,
                message = "User identity is invalid."
            });
        }

        bool deleted;

        try
        {
            deleted =
                await _assignmentService.DeleteAssignmentAsync(
                    userId,
                    id);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }

        if (!deleted)
        {
            return NotFound(new
            {
                success = false,
                message = "Assignment not found."
            });
        }

        return Ok(new
        {
            success = true,
            message = "Assignment deleted successfully."
        });
    }

    [HttpPost("{id:int}/publish")]
    [Authorize(Roles = "Teacher")]
    public async Task<IActionResult> Publish(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                success = false,
                message = "Invalid assignment id."
            });
        }

        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized(new
            {
                success = false,
                message = "User identity is invalid."
            });
        }

        AssignmentResponse? result;

        try
        {
            result =
                await _assignmentService.PublishAssignmentAsync(
                    userId,
                    id);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message
            });
        }

        if (result == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Assignment not found."
            });
        }

        return Ok(result);
    }

    private bool TryGetCurrentUserId(out int userId)
    {
        userId = 0;

        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier) ??
            User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return !string.IsNullOrWhiteSpace(userIdClaim) &&
            int.TryParse(userIdClaim, out userId);
    }
}
