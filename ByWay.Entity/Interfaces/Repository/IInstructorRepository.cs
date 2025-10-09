using ByWay.Domain.Entities;

namespace ByWay.Domain.Interfaces.Repository;

public interface IInstructorRepository : IRepository<Instructor>
{
  Task<bool> HasCoursesAsync(int id);
}