using System.ComponentModel.DataAnnotations;

namespace AssignmentManagement.DTOs.Assignments;

public class CreateAssignmentRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Teacher assignment id must be greater than 0.")]
    public int TeacherAssignmentId { get; set; }

    [Required(ErrorMessage = "Assignment title is required.")]
    [RegularExpression(@".*\S.*", ErrorMessage = "Assignment title cannot be empty.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "Assignment title must be between 3 and 150 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Description must not exceed 1000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Deadline is required.")]
    public DateTime Deadline { get; set; }

    [Range(1, 1000, ErrorMessage = "Maximum marks must be between 1 and 1000.")]
    public int MaxMarks { get; set; }
}
