using ByWay.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


namespace ByWay.Infrastructure.Data.Contexts.AppContext
{
  public class AppDbContext : IdentityDbContext<AppUser>
  {
    public DbSet<Course> Courses { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CourseContent> Contents { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    protected AppDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
      base.OnModelCreating(modelBuilder);
    }
  }
}
