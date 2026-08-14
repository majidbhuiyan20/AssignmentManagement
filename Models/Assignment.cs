public class Assignment
{
    public int Id {get; set;}
    public string Title {get; set;} = string.Empty;
    public string Description {get; set;} = string.Empty;
    public DateTime Deadline {get; set;}
    public int MaxMarks {get; set;}
    public AssignmentStatus Status {get; set;} = AssignmentStatus.Draft;
    public int TeacherAssignmentId {get; set;}  // Use as foreign key to the TeacherAssignment table
    public TeacherAssignment TeacherAssignment {get; set;} = null!; // Navigation property to the TeacherAssignment entity representing the teacher assignment
    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;
}