using AssignmentManagement.DTOs.Subjects;

namespace AssignmentManagement.Interfaces;

public interface ISubjectService
{
    Task<List<SubjectResponse>> GetAllSubjectsAsync();

    Task<SubjectResponse?> GetSubjectByIdAsync(int id);

    Task<SubjectResponse> CreateSubjectAsync(
        CreateSubjectRequest request);

    Task<SubjectResponse?> UpdateSubjectAsync(
        int id,
        UpdateSubjectRequest request);

    Task<bool> DeleteSubjectAsync(int id);
}