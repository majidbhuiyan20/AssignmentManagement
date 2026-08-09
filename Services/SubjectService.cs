using AssignmentManagement.Data;
using AssignmentManagement.DTOs.Subjects;
using AssignmentManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AssignmentManagement.Services;

public class SubjectService : ISubjectService
{
    private readonly ApplicationDbContext _context;

    public SubjectService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SubjectResponse>> GetAllSubjectsAsync()
    {
        return await _context.Subjects
            .Select(subject => new SubjectResponse
            {
                Id = subject.Id,
                Name = subject.Name
            })
            .ToListAsync();
    }

    public async Task<SubjectResponse?> GetSubjectByIdAsync(int id)
    {
        return await _context.Subjects
            .Where(subject => subject.Id == id)
            .Select(subject => new SubjectResponse
            {
                Id = subject.Id,
                Name = subject.Name
            })
            .FirstOrDefaultAsync();
    }

    public async Task<SubjectResponse> CreateSubjectAsync(
        CreateSubjectRequest request)
    {
        bool subjectExists = await _context.Subjects
            .AnyAsync(subject => subject.Name == request.Name);

        if (subjectExists)
        {
            throw new Exception("Subject already exists.");
        }

        var subject = new Subject
        {
            Name = request.Name
        };

        _context.Subjects.Add(subject);

        await _context.SaveChangesAsync();

        return new SubjectResponse
        {
            Id = subject.Id,
            Name = subject.Name
        };
    }

    public async Task<SubjectResponse?> UpdateSubjectAsync(
        int id,
        UpdateSubjectRequest request)
    {
        var subject = await _context.Subjects
            .FirstOrDefaultAsync(subject => subject.Id == id);

        if (subject == null)
        {
            return null;
        }

        bool duplicate = await _context.Subjects
            .AnyAsync(subject =>
                subject.Name == request.Name &&
                subject.Id != id);

        if (duplicate)
        {
            throw new Exception("Subject already exists.");
        }

        subject.Name = request.Name;

        await _context.SaveChangesAsync();

        return new SubjectResponse
        {
            Id = subject.Id,
            Name = subject.Name
        };
    }

    public async Task<bool> DeleteSubjectAsync(int id)
    {
        var subject = await _context.Subjects
            .FirstOrDefaultAsync(subject => subject.Id == id);

        if (subject == null)
        {
            return false;
        }

        _context.Subjects.Remove(subject);

        await _context.SaveChangesAsync();

        return true;
    }
}