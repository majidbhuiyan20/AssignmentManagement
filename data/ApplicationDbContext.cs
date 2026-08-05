
using Microsoft.EntityFrameworkCore;
namespace AssignmentManagement.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options
    ): base(options)
    {}
    public DbSet<User> Users { get; set; }
    public DbSet<Class> Classes { get; set; }
}