using AssignmentManagement.Data;
using AssignmentManagement.DTOs.Assignments;
using AssignmentManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AssignmentManagement.Services;

public class AssignmentService : IAssignmentService
{
    private readonly ApplicationDbContext _context;

    public AssignmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AssignmentResponse> CreateAssignmentAsync(
        int userId,
        CreateAssignmentRequest request)
    {
        var teacherAssignment = await GetTeacherAssignmentAsync(
            request.TeacherAssignmentId);

        if (teacherAssignment == null)
        {
            throw new KeyNotFoundException(
                "Teacher assignment not found.");
        }

        EnsureTeacherOwnsAssignment(
            teacherAssignment,
            userId,
            "You are not allowed to create an assignment for this teacher assignment.");

        ValidateAssignmentRequest(
            request.Title,
            request.Deadline,
            request.MaxMarks);

        var assignment = new Assignment
        {
            TeacherAssignmentId = request.TeacherAssignmentId,
            Title = request.Title.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
            Deadline = request.Deadline,
            MaxMarks = request.MaxMarks,
            Status = AssignmentStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };

        _context.Assignments.Add(assignment);

        await _context.SaveChangesAsync();

        return MapToResponse(
            assignment,
            teacherAssignment);
    }

    public async Task<AssignmentResponse?> GetAssignmentByIdAsync(int id)
    {
        var assignment = await GetAssignmentWithDetailsQuery()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (assignment == null)
        {
            return null;
        }

        return MapToResponse(
            assignment,
            assignment.TeacherAssignment);
    }

    public async Task<List<AssignmentResponse>> GetAllAssignmentsAsync(
        int userId)
    {
        var assignments = await GetAssignmentWithDetailsQuery()
            .Where(a => a.TeacherAssignment.TeacherId == userId)
            .ToListAsync();

        return assignments
            .Select(a => MapToResponse(
                a,
                a.TeacherAssignment))
            .ToList();
    }

    public async Task<AssignmentResponse?> UpdateAssignmentAsync(
        int userId,
        int id,
        UpdateAssignmentRequest request)
    {
        var assignment = await GetAssignmentWithDetailsQuery()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (assignment == null)
        {
            return null;
        }

        EnsureTeacherOwnsAssignment(
            assignment.TeacherAssignment,
            userId,
            "You are not allowed to update this assignment.");

        ValidateAssignmentRequest(
            request.Title,
            request.Deadline,
            request.MaxMarks);

        assignment.Title = request.Title.Trim();
        assignment.Description = request.Description?.Trim() ?? string.Empty;
        assignment.Deadline = request.Deadline;
        assignment.MaxMarks = request.MaxMarks;

        await _context.SaveChangesAsync();

        return MapToResponse(
            assignment,
            assignment.TeacherAssignment);
    }

    public async Task<bool> DeleteAssignmentAsync(
        int userId,
        int id)
    {
        var assignment = await _context.Assignments
            .Include(a => a.TeacherAssignment)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (assignment == null)
        {
            return false;
        }

        EnsureTeacherOwnsAssignment(
            assignment.TeacherAssignment,
            userId,
            "You are not allowed to delete this assignment.");

        _context.Assignments.Remove(assignment);

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<AssignmentResponse?> PublishAssignmentAsync(
        int userId,
        int id)
    {
        var assignment = await GetAssignmentWithDetailsQuery()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (assignment == null)
        {
            return null;
        }

        EnsureTeacherOwnsAssignment(
            assignment.TeacherAssignment,
            userId,
            "You are not allowed to publish this assignment.");

        if (assignment.Status == AssignmentStatus.published)
        {
            throw new InvalidOperationException(
                "Assignment is already published.");
        }

        assignment.Status = AssignmentStatus.published;

        await _context.SaveChangesAsync();

        return MapToResponse(
            assignment,
            assignment.TeacherAssignment);
    }

    private async Task<TeacherAssignment?> GetTeacherAssignmentAsync(
        int teacherAssignmentId)
    {
        return await _context.TeacherAssignments
            .Include(ta => ta.Teacher)
            .Include(ta => ta.AcademicClass)
            .Include(ta => ta.Subject)
            .FirstOrDefaultAsync(ta => ta.Id == teacherAssignmentId);
    }

    private IQueryable<Assignment> GetAssignmentWithDetailsQuery()
    {
        return _context.Assignments
            .Include(a => a.TeacherAssignment)
                .ThenInclude(ta => ta.Teacher)
            .Include(a => a.TeacherAssignment)
                .ThenInclude(ta => ta.AcademicClass)
            .Include(a => a.TeacherAssignment)
                .ThenInclude(ta => ta.Subject);
    }

    private static void EnsureTeacherOwnsAssignment(
        TeacherAssignment teacherAssignment,
        int userId,
        string message)
    {
        if (teacherAssignment.TeacherId != userId)
        {
            throw new UnauthorizedAccessException(message);
        }
    }

    private static void ValidateAssignmentRequest(
        string title,
        DateTime deadline,
        int maxMarks)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException(
                "Assignment title is required.");
        }

        if (maxMarks <= 0)
        {
            throw new ArgumentException(
                "Maximum marks must be greater than zero.");
        }

        if (deadline <= DateTime.UtcNow)
        {
            throw new ArgumentException(
                "Deadline must be in the future.");
        }
    }

    private static AssignmentResponse MapToResponse(
        Assignment assignment,
        TeacherAssignment teacherAssignment)
    {
        return new AssignmentResponse
        {
            Id = assignment.Id,
            Title = assignment.Title,
            Description = assignment.Description,
            Deadline = assignment.Deadline,
            MaxMarks = assignment.MaxMarks,
            Status = assignment.Status.ToString(),
            TeacherAssignmentId = assignment.TeacherAssignmentId,
            TeacherId = teacherAssignment.TeacherId,
            TeacherName = teacherAssignment.Teacher.FullName,
            AcademicClassId = teacherAssignment.AcademicClassId,
            ClassName = teacherAssignment.AcademicClass.Name,
            SubjectId = teacherAssignment.SubjectId,
            SubjectName = teacherAssignment.Subject.Name,
            CreatedAt = assignment.CreatedAt
        };
    }
}
