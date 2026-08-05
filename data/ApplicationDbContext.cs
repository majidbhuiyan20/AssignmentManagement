
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
}