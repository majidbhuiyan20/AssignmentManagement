using System.ComponentModel.DataAnnotations;

namespace AssignmentManagement.DTOs.TeacherAssignments;

public class CreateTeacherAssignmentRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Teacher id must be greater than 0.")]
    public int TeacherId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Academic class id must be greater than 0.")]
    public int AcademicClassId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Subject id must be greater than 0.")]
    public int SubjectId { get; set; }
}
