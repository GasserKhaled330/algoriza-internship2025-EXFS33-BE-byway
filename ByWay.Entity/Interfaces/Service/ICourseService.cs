using ByWay.Domain.Dtos.Course;
using ByWay.Domain.DTOs.Course;
using ByWay.Domain.Entities;
using ByWay.Domain.Specifications;

namespace ByWay.Domain.Interfaces.Service;

public interface ICourseService
{
  Task<(IReadOnlyList<Course>, int TotalCount)> GetPagedCoursesAsync(
      int pageIndex,
      int pageSize,
      string? sortBy = null,
      string? name = null,
      string? category = null,
      double? rating = null,
      decimal? cost = null,
      int? minLecturesCount = null,
      int? maxLecturesCount = null);
  Task<Course?> GetCourseByIdAsync(int id);
  Task<IReadOnlyList<Course>> GetTopRatedCoursesAsync(ISpecification<Course> spec);
  Task<IReadOnlyList<Course>> GetTopRatedCoursesInSameCategoryAsync(ISpecification<Course> spec);
  Task<int> GetCoursesCountAsync();
  Task<Course> AddCourseAsync(AddCourseDto course);
  Task UpdateCourseAsync(int id, UpdateCourseDto updatedCourseDto);
  Task RemoveCourseAsync(int id);
}