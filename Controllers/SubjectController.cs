using AssignmentManagement.DTOs.Subjects;
using AssignmentManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssignmentManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class SubjectController : ControllerBase
{
    private readonly ISubjectService _subjectService;

    public SubjectController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllSubjects()
    {
        var subjects = await _subjectService.GetAllSubjectsAsync();

        return Ok(subjects);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubjectById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                message = "Invalid subject id."
            });
        }

        var subject = await _subjectService
            .GetSubjectByIdAsync(id);

        if (subject == null)
        {
            return NotFound(new
            {
                message = "Subject not found."
            });
        }

        return Ok(subject);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubject(
        CreateSubjectRequest request)
    {
        SubjectResponse subject;

        try
        {
            subject = await _subjectService
                .CreateSubjectAsync(request);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }

        return CreatedAtAction(
            nameof(GetSubjectById),
            new { id = subject.Id },
            subject);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubject(
        int id,
        UpdateSubjectRequest request)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                message = "Invalid subject id."
            });
        }

        SubjectResponse? subject;

        try
        {
            subject = await _subjectService
                .UpdateSubjectAsync(id, request);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }

        if (subject == null)
        {
            return NotFound(new
            {
                message = "Subject not found."
            });
        }

        return Ok(subject);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        if (id <= 0)
        {
            return BadRequest(new
            {
                message = "Invalid subject id."
            });
        }

        bool deleted = await _subjectService
            .DeleteSubjectAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Subject not found."
            });
        }

        return Ok(new
        {
            message = "Subject deleted successfully."
        });
    }
}
