using System.ComponentModel.DataAnnotations;

namespace AssignmentManagement.DTOs.Users;

public class CreateUserRequest
{
    [Required(ErrorMessage = "Full name is required.")]
    [RegularExpression(@".*\S.*", ErrorMessage = "Full name cannot be empty.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 100 characters.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email address is not valid.")]
    [StringLength(150, ErrorMessage = "Email must not exceed 150 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression(@".*\S.*", ErrorMessage = "Password cannot be empty.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required.")]
    [RegularExpression("(?i)^(Admin|Teacher|Student)$", ErrorMessage = "Role must be Admin, Teacher, or Student.")]
    public string Role { get; set; } = string.Empty;
}
