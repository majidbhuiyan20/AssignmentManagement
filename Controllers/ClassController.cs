using AssignmentManagement.DTOs.Classes;
using AssignmentManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class ClassController : ControllerBase
{
    private readonly IClassService _classService;

    public ClassController(IClassService classService)
    {
        _classService = classService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClasses()
    {
        var classes = await _classService.GetAllClassesAsync();

        return Ok(classes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetClassById(int id)
    {
        var academicClass =
            await _classService.GetClassByIdAsync(id);

        if (academicClass == null)
        {
            return NotFound(new
            {
                message = "Class not found."
            });
        }

        return Ok(academicClass);
    }

    [HttpPost]
    public async Task<IActionResult> CreateClass(
        CreateClassRequest request)
    {
        var academicClass =
            await _classService.CreateClassAsync(request);

        return CreatedAtAction(
            nameof(GetClassById),
            new { id = academicClass.Id },
            academicClass);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClass(
        int id,
        UpdateClassRequest request)
    {
        var academicClass =
            await _classService.UpdateClassAsync(id, request);

        if (academicClass == null)
        {
            return NotFound(new
            {
                message = "Class not found."
            });
        }

        return Ok(academicClass);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClass(int id)
    {
        var deleted =
            await _classService.DeleteClassAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Class not found."
            });
        }

        return Ok(new
        {
            message = "Class deleted successfully."
        });
    }
}