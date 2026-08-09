using AssignmentManagement.Data;
using AssignmentManagement.DTOs.Classes;
using AssignmentManagement.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AssignmentManagement.Services;

public class ClassService : IClassService
{
    private readonly ApplicationDbContext _context;

    public ClassService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ClassResponse>> GetAllClassesAsync()
    {
        return await _context.AcademicClasses
            .Select(c => new ClassResponse
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();
    }

    public async Task<ClassResponse?> GetClassByIdAsync(int id)
    {
        return await _context.AcademicClasses
            .Where(c => c.Id == id)
            .Select(c => new ClassResponse
            {
                Id = c.Id,
                Name = c.Name
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ClassResponse> CreateClassAsync(
        CreateClassRequest request)
    {
        var classExists = await _context.AcademicClasses
            .AnyAsync(c => c.Name == request.Name);

        if (classExists)
        {
            throw new Exception("Class already exists.");
        }

        var academicClass = new AcademicClass
        {
            Name = request.Name
        };

        _context.AcademicClasses.Add(academicClass);

        await _context.SaveChangesAsync();

        return new ClassResponse
        {
            Id = academicClass.Id,
            Name = academicClass.Name
        };
    }

    public async Task<ClassResponse?> UpdateClassAsync(
        int id,
        UpdateClassRequest request)
    {
        var academicClass = await _context.AcademicClasses
            .FirstOrDefaultAsync(c => c.Id == id);

        if (academicClass == null)
        {
            return null;
        }

        var duplicate = await _context.AcademicClasses
            .AnyAsync(c =>
                c.Name == request.Name &&
                c.Id != id);

        if (duplicate)
        {
            throw new Exception("Class already exists.");
        }

        academicClass.Name = request.Name;

        await _context.SaveChangesAsync();

        return new ClassResponse
        {
            Id = academicClass.Id,
            Name = academicClass.Name
        };
    }

    public async Task<bool> DeleteClassAsync(int id)
    {
        var academicClass = await _context.AcademicClasses
            .FirstOrDefaultAsync(c => c.Id == id);

        if (academicClass == null)
        {
            return false;
        }

        _context.AcademicClasses.Remove(academicClass);

        await _context.SaveChangesAsync();

        return true;
    }
}