
using Microsoft.EntityFrameworkCore;
namespace AssignmentManagement.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options
    ): base(options)
    {}
    public DbSet<User> Users { get; set; }
    public DbSet<AcademicClass> AcademicClasses { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<TeacherAssignment> TeacherAssignments { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<Submission> Submissions { get; set; }
}