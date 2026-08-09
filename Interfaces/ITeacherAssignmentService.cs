using AssignmentManagement.DTOs.TeacherAssignments;

namespace AssignmentManagement.Interfaces;

public interface ITeacherAssignmentService
{
    Task<List<TeacherAssignmentResponse>>
        GetAllTeacherAssignmentsAsync();

    Task<TeacherAssignmentResponse?>
        GetTeacherAssignmentByIdAsync(int id);

    Task<TeacherAssignmentResponse>
        CreateTeacherAssignmentAsync(
            CreateTeacherAssignmentRequest request);

    Task<bool>
        DeleteTeacherAssignmentAsync(int id);
}