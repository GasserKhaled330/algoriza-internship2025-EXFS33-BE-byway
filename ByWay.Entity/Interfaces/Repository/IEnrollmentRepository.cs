using ByWay.Domain.Entities;

namespace ByWay.Domain.Interfaces.Repository;

public interface IEnrollmentRepository : IRepository<Enrollment>
{
  Task<bool> IsEnrolledAsync(string userId, int courseId);
  Task<bool> AnyForCourseAsync(int courseId);
}