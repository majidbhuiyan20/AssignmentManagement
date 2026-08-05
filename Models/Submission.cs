public class Submission
{
    public int Id{get; set;} // here we are using Id as primary key for the submission table(id is auto incremented by default in EF core)
    public int AssignmentId{get; set;} // Use as foreign key to the Assignment table
    public Assignment Assignment{get; set;} = null!; // Navigation property to the Assignment entity representing the assignment
    public int StudentId{get; set;} // Use as foreign key to the User table
    public User Student{get; set;} = null!; // Navigation property to the User entity representing the student
    public string Answer{get; set;} = string.Empty;
    public DateTime SubmittedAt{get; set;} = DateTime.UtcNow;
    public double? Marks {get; set;} // Marks can be null if the submission has not been graded yet
    public string? Feedback {get; set;} // Feedback can be null if the submission has not been graded yet
    public SubmissionStatus Status {get; set;} = SubmissionStatus.Pending; // Default status is Pending
}