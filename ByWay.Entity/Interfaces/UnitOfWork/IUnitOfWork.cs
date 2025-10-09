using ByWay.Domain.Interfaces.Repository;

namespace ByWay.Domain.Interfaces.UnitOfWork;

public interface IUnitOfWork
{
  IInstructorRepository Instructors { get; }
  ICategoriesRepository Categories { get; }
  ICourseRepository Courses { get; }
  ICourseContentRepository CourseContents { get; }
  IEnrollmentRepository Enrollments { get; }
  IPaymentRepository Payments { get; }
  ICartRepository Carts { get; }

  Task<int> CompleteAsync();
}