using ByWay.Domain.Interfaces.Repository;
using ByWay.Domain.Interfaces.UnitOfWork;
using ByWay.Infrastructure.Data.Contexts.AppContext;
using ByWay.Infrastructure.Repositories;

namespace ByWay.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
  private readonly AppDbContext _context;
  private IInstructorRepository? _instructors;
  private ICategoriesRepository? _categories;
  private ICourseRepository? _courses;
  private ICourseContentRepository? _courseContents;
  private IEnrollmentRepository? _enrollments;
  private IPaymentRepository? _payments;
  private ICartRepository? _carts;

  public UnitOfWork(AppDbContext context)
  {
    _context = context;
  }

  public IInstructorRepository Instructors => _instructors ??= new InstructorRepository(_context);

  public ICategoriesRepository Categories =>
      _categories ??= new CategoriesRepository(_context);

  public ICourseRepository Courses => _courses ??= new CourseRepository(_context);

  public ICourseContentRepository CourseContents =>
      _courseContents ??= new CourseContentRepository(_context);

  public IEnrollmentRepository Enrollments => _enrollments ??= new EnrollmentRepository(_context);

  public IPaymentRepository Payments => _payments ??= new PaymentRepository(_context);

  public ICartRepository Carts => _carts ??= new CartRepository(_context);

  public async Task<int> CompleteAsync()
        => await _context.SaveChangesAsync();

}