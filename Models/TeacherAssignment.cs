public class TeacherAssignment
{
    public int Id { get; set; }
    public int TeacherId { get; set; } // Use as foreign key to the User table
    public User Teacher { get; set; } = null!; // Navigation property to the User entity representing the teacher
    public int AcademicClassId { get; set; } // Use as foreign key to the AcademicClass table
    public AcademicClass AcademicClass { get; set; } = null!; // Navigation property to the AcademicClass entity representing the class
    public int SubjectId { get; set; } // Use as foreign key to the Subject table
    public Subject Subject { get; set; } = null!; // Navigation property to the Subject entity representing the subject
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    

}