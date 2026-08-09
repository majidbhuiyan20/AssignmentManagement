using AssignmentManagement.DTOs.Classes;

namespace AssignmentManagement.Interfaces;

public interface IClassService
{
    Task<List<ClassResponse>> GetAllClassesAsync();

    Task<ClassResponse?> GetClassByIdAsync(int id);

    Task<ClassResponse> CreateClassAsync(
        CreateClassRequest request);

    Task<ClassResponse?> UpdateClassAsync(
        int id,
        UpdateClassRequest request);

    Task<bool> DeleteClassAsync(int id);
}