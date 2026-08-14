using AssignmentManagement.DTOs.Assignments;

namespace AssignmentManagement.Interfaces;

public interface IAssignmentService
{
    Task<AssignmentResponse> CreateAssignmentAsync(
        int userId,
        CreateAssignmentRequest request);

    Task<AssignmentResponse?> GetAssignmentByIdAsync(
        int id);

    Task<List<AssignmentResponse>> GetAllAssignmentsAsync(
        int userId);

    Task<AssignmentResponse?> UpdateAssignmentAsync(
        int userId,
        int id,
        UpdateAssignmentRequest request);

    Task<bool> DeleteAssignmentAsync(
        int userId,
        int id);

    Task<AssignmentResponse?> PublishAssignmentAsync(
        int userId,
        int id);
}