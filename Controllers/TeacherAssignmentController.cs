using AssignmentManagement.DTOs.TeacherAssignments;
using AssignmentManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class TeacherAssignmentController
    : ControllerBase
{
    private readonly ITeacherAssignmentService
        _teacherAssignmentService;

    public TeacherAssignmentController(
        ITeacherAssignmentService teacherAssignmentService)
    {
        _teacherAssignmentService =
            teacherAssignmentService;
    }

    [HttpGet]
    public async Task<IActionResult>
        GetAllTeacherAssignments()
    {
        var assignments =
            await _teacherAssignmentService
                .GetAllTeacherAssignmentsAsync();

        return Ok(assignments);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult>
        GetTeacherAssignmentById(int id)
    {
        var assignment =
            await _teacherAssignmentService
                .GetTeacherAssignmentByIdAsync(id);

        if (assignment == null)
        {
            return NotFound(new
            {
                message = "Teacher assignment not found."
            });
        }

        return Ok(assignment);
    }

    [HttpPost]
    public async Task<IActionResult>
        CreateTeacherAssignment(
            CreateTeacherAssignmentRequest request)
    {
        var assignment =
            await _teacherAssignmentService
                .CreateTeacherAssignmentAsync(request);

        return CreatedAtAction(
            nameof(GetTeacherAssignmentById),
            new { id = assignment.Id },
            assignment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult>
        DeleteTeacherAssignment(int id)
    {
        bool deleted =
            await _teacherAssignmentService
                .DeleteTeacherAssignmentAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message =
                    "Teacher assignment not found."
            });
        }

        return Ok(new
        {
            message =
                "Teacher assignment deleted successfully."
        });
    }
}