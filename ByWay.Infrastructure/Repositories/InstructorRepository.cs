using ByWay.Domain.Entities;
using ByWay.Domain.Interfaces.Repository;
using ByWay.Infrastructure.Data.Contexts.AppContext;
using Microsoft.EntityFrameworkCore;

namespace ByWay.Infrastructure.Repositories;

public class InstructorRepository : Repository<Instructor>, IInstructorRepository
{
  public InstructorRepository(AppDbContext context) : base(context)
  {
  }

  public async Task<bool> HasCoursesAsync(int id)
  {
    return await _dbSet
        .AsNoTracking()
        .AnyAsync(i => i.Id == id && i.Courses.Any());
  }
}