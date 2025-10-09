using ByWay.Domain.Entities;
using ByWay.Domain.Interfaces.Repository;
using ByWay.Infrastructure.Data.Contexts.AppContext;
using Microsoft.EntityFrameworkCore;

namespace ByWay.Infrastructure.Repositories;

public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
{
  public EnrollmentRepository(AppDbContext context) : base(context)
  {
  }

  public async Task<bool> IsEnrolledAsync(string userId, int courseId)
  {
    return await _dbSet.AsNoTracking().Where(e => e.AppUserId == userId && e.CourseId == courseId).AnyAsync();
  }

  public async Task<bool> AnyForCourseAsync(int courseId)
  {
    return await _dbSet.AsNoTracking().Where(e => e.CourseId == courseId).AnyAsync();
  }
}