namespace AssignmentManagement.DTOs.TeacherAssignments;

public class CreateTeacherAssignmentRequest
{
    public int TeacherId { get; set; }

    public int AcademicClassId { get; set; }

    public int SubjectId { get; set; }
}