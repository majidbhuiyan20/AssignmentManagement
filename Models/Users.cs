public class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Teacher -> Many TeacherAssignments
    public ICollection<TeacherAssignment> TeacherAssignments { get; set; }
        = new List<TeacherAssignment>();

    // Student -> Many Submissions
    public ICollection<Submission> Submissions { get; set; }
        = new List<Submission>();
}