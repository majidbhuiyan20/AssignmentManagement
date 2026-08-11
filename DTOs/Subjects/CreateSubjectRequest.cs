using System.ComponentModel.DataAnnotations;

namespace AssignmentManagement.DTOs.Subjects;

public class CreateSubjectRequest
{
    [Required(ErrorMessage = "Subject name is required.")]
    [RegularExpression(@".*\S.*", ErrorMessage = "Subject name cannot be empty.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Subject name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;
}
