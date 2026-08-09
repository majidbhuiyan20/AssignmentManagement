using AssignmentManagement.Data;
using AssignmentManagement.DTOs.TeacherAssignments;
using AssignmentManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AssignmentManagement.Services;

public class TeacherAssignmentService
    : ITeacherAssignmentService
{
    private readonly ApplicationDbContext _context;

    public TeacherAssignmentService(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TeacherAssignmentResponse>>
        GetAllTeacherAssignmentsAsync()
    {
        return await _context.TeacherAssignments
            .Include(ta => ta.Teacher)
            .Include(ta => ta.AcademicClass)
            .Include(ta => ta.Subject)
            .Select(ta => new TeacherAssignmentResponse
            {
                Id = ta.Id,

                TeacherId = ta.TeacherId,
                TeacherName = ta.Teacher.FullName,

                AcademicClassId = ta.AcademicClassId,
                ClassName = ta.AcademicClass.Name,

                SubjectId = ta.SubjectId,
                SubjectName = ta.Subject.Name
            })
            .ToListAsync();
            }
            public async Task<TeacherAssignmentResponse?>
    GetTeacherAssignmentByIdAsync(int id)
{
    return await _context.TeacherAssignments
        .Include(ta => ta.Teacher)
        .Include(ta => ta.AcademicClass)
        .Include(ta => ta.Subject)
        .Where(ta => ta.Id == id)
        .Select(ta => new TeacherAssignmentResponse
        {
            Id = ta.Id,

            TeacherId = ta.TeacherId,
            TeacherName = ta.Teacher.FullName,

            AcademicClassId = ta.AcademicClassId,
            ClassName = ta.AcademicClass.Name,

            SubjectId = ta.SubjectId,
            SubjectName = ta.Subject.Name
        })
        .FirstOrDefaultAsync();
}   
    public async Task<TeacherAssignmentResponse>
    CreateTeacherAssignmentAsync(
        CreateTeacherAssignmentRequest request)
{
    var teacher = await _context.Users
        .FirstOrDefaultAsync(u =>
            u.Id == request.TeacherId &&
            u.Role == UserRole.Teacher);

    if (teacher == null)
    {
        throw new Exception(
            "Teacher not found.");
    }

    var academicClass = await _context.AcademicClasses
        .FirstOrDefaultAsync(c =>
            c.Id == request.AcademicClassId);

    if (academicClass == null)
    {
        throw new Exception(
            "Class not found.");
    }

    var subject = await _context.Subjects
        .FirstOrDefaultAsync(s =>
            s.Id == request.SubjectId);

    if (subject == null)
    {
        throw new Exception(
            "Subject not found.");
    }

    bool alreadyAssigned =
        await _context.TeacherAssignments.AnyAsync(ta =>
            ta.TeacherId == request.TeacherId &&
            ta.AcademicClassId == request.AcademicClassId &&
            ta.SubjectId == request.SubjectId);

    if (alreadyAssigned)
    {
        throw new Exception(
            "This teacher is already assigned to this class and subject.");
    }

    var teacherAssignment = new TeacherAssignment
    {
        TeacherId = request.TeacherId,
        AcademicClassId = request.AcademicClassId,
        SubjectId = request.SubjectId
    };

    _context.TeacherAssignments.Add(teacherAssignment);

    await _context.SaveChangesAsync();

    return new TeacherAssignmentResponse
    {
        Id = teacherAssignment.Id,

        TeacherId = teacher.Id,
        TeacherName = teacher.FullName,

        AcademicClassId = academicClass.Id,
        ClassName = academicClass.Name,

        SubjectId = subject.Id,
        SubjectName = subject.Name
    };
}

public async Task<bool>
    DeleteTeacherAssignmentAsync(int id)
{
    var teacherAssignment =
        await _context.TeacherAssignments
            .FirstOrDefaultAsync(ta => ta.Id == id);

    if (teacherAssignment == null)
    {
        return false;
    }

    _context.TeacherAssignments.Remove(
        teacherAssignment);

    await _context.SaveChangesAsync();

    return true;
}

    }