using System.ComponentModel.DataAnnotations;

namespace AssignmentManagement.DTOs.Classes;

public class CreateClassRequest
{
    [Required(ErrorMessage = "Class name is required.")]
    [RegularExpression(@".*\S.*", ErrorMessage = "Class name cannot be empty.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Class name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;
}
