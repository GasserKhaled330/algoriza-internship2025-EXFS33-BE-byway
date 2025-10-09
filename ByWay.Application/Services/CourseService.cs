using ByWay.Application.Specifications.CourseSpecification;
using ByWay.Domain.Dtos.Course;
using ByWay.Domain.DTOs.Course;
using ByWay.Domain.Entities;
using ByWay.Domain.Interfaces.Service;
using ByWay.Domain.Interfaces.UnitOfWork;
using ByWay.Domain.Specifications;

namespace ByWay.Application.Services;

public class CourseService : ICourseService
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IImageService _imageService;
  private const string CourseImagesFolder = "images/courses/";

  public CourseService(IUnitOfWork unitOfWork, IImageService imageService)
  {
    _unitOfWork = unitOfWork;
    _imageService = imageService;
  }


  public async Task<(IReadOnlyList<Course>, int TotalCount)> GetPagedCoursesAsync(
      int pageIndex,
      int pageSize,
      string? sortBy = null,
      string? name = null,
      string? category = null,
      double? rating = null,
      decimal? cost = null,
      int? minLecturesCount = null,
      int? maxLecturesCount = null
  )
  {
    var pagedSpec = new PagedCourseFilterSpecification(pageIndex, pageSize, sortBy, name, category, rating, cost,
        minLecturesCount, maxLecturesCount);
    var filterSpec = new CourseFilterSpecification(name, category, rating, cost, minLecturesCount, maxLecturesCount);

    var courses = await _unitOfWork.Courses.GetAllBySpecAsync(pagedSpec);
    var totalCount = await _unitOfWork.Courses.CountAsync(filterSpec);

    return (courses, totalCount);
  }

  public async Task<Course?> GetCourseByIdAsync(int id)
  {
    var spec = new CourseWithContentsSpecification(id);
    return await _unitOfWork.Courses.GetBySpecAsync(spec);
  }

  public async Task<IReadOnlyList<Course>> GetTopRatedCoursesAsync(ISpecification<Course> spec)
  {
    return await _unitOfWork.Courses.GetTopRatedCoursesAsync(spec);
  }

  public async Task<IReadOnlyList<Course>> GetTopRatedCoursesInSameCategoryAsync(ISpecification<Course> spec)
  {
    return await _unitOfWork.Courses.GetTopRatedCoursesInSameCategoryAsync(spec);
  }

  public async Task<Course> AddCourseAsync(AddCourseDto course)
  {
    var imagePath = await _imageService.UploadImageAsync(
        course.Image,
        CourseImagesFolder
    );
    var newCourse = new Course
    {
      Name = course.Name,
      Description = course.Description,
      Certification = course.Certification,
      Cost = course.Cost,
      Level = course.Level,
      Rate = course.Rate,
      TotalHours = course.TotalHours,
      ImagePath = imagePath,
      InstructorId = course.InstructorId,
      CategoryId = course.CategoryId,
      Contents = course.Contents.Select(content => new CourseContent
      {
        Name = content.Name,
        LecturesCount = content.LecturesCount,
        DurationInHours = content.DurationInHours
      }).ToList()
    };
    await _unitOfWork.Courses.AddAsync(newCourse);
    await _unitOfWork.CompleteAsync();
    return newCourse;
  }

  public async Task UpdateCourseAsync(int id, UpdateCourseDto updatedCourseDto)
  {
    var spec = new CourseWithContentsSpecification(id);
    var course = await _unitOfWork.Courses.GetBySpecAsync(spec) ??
                 throw new KeyNotFoundException($"Course with id {id} not found.");

    if (updatedCourseDto.Name is not null)
      course.Name = updatedCourseDto.Name;
    if (updatedCourseDto.Description is not null)
      course.Description = updatedCourseDto.Description;
    if (updatedCourseDto.Certification is not null)
      course.Certification = updatedCourseDto.Certification;
    if (updatedCourseDto.Cost.HasValue)
      course.Cost = updatedCourseDto.Cost.Value;
    if (updatedCourseDto.Level.HasValue)
      course.Level = updatedCourseDto.Level.Value;
    if (updatedCourseDto.Rate.HasValue)
      course.Rate = updatedCourseDto.Rate.Value;
    if (updatedCourseDto.TotalHours.HasValue)
      course.TotalHours = updatedCourseDto.TotalHours.Value;
    if (updatedCourseDto.InstructorId.HasValue)
      course.InstructorId = updatedCourseDto.InstructorId.Value;
    if (updatedCourseDto.CategoryId.HasValue)
      course.CategoryId = updatedCourseDto.CategoryId.Value;

    if (updatedCourseDto.Image is not null && updatedCourseDto.Image.Length > 0)
    {
      var newImagePath = await _imageService.UploadImageAsync(
          updatedCourseDto.Image,
          CourseImagesFolder
      );
      // delete the old image if it exists
      _imageService.DeleteImage(course.ImagePath);
      course.ImagePath = newImagePath;
    }

    if (updatedCourseDto.Contents is not null)
    {
      // course.Contents = updatedCourse.Contents.Select(content => new CourseContent
      // {
      //     Name = content.Name,
      //     LecturesCount = content.LecturesCount,
      //     DurationInHours = content.DurationInHours
      // }).ToList();

      var updatedContentsMap = updatedCourseDto.Contents
          .ToDictionary(dto => dto.Id, dto => dto);

      var existingContents = course.Contents.ToList();

      var contentsToDelete = existingContents
          .Where(c => !updatedContentsMap.ContainsKey(c.Id))
          .ToList();
      foreach (var content in contentsToDelete)
      {
        course.Contents.Remove(content);
      }

      foreach (var contentDto in updatedCourseDto.Contents)
      {
        var existingContent = existingContents.FirstOrDefault(c => c.Id == contentDto.Id);
        if (existingContent != null)
        {
          if (existingContent.Name != contentDto.Name)
            existingContent.Name = contentDto.Name;
          if (existingContent.LecturesCount != contentDto.LecturesCount)
            existingContent.LecturesCount = contentDto.LecturesCount;
          if (existingContent.DurationInHours != contentDto.DurationInHours)
            existingContent.DurationInHours = contentDto.DurationInHours;
        }
        else
        {
          course.Contents.Add(new CourseContent
          {
            Name = contentDto.Name,
            LecturesCount = contentDto.LecturesCount,
            DurationInHours = contentDto.DurationInHours
          });
        }
      }
    }

    await _unitOfWork.CompleteAsync();
  }

  public async Task RemoveCourseAsync(int id)
  {
    var spec = new CourseWithContentsSpecification(id);
    var course = await _unitOfWork.Courses.GetBySpecAsync(spec) ??
                 throw new KeyNotFoundException($"Course with id {id} not found.");

    if (await _unitOfWork.Enrollments.AnyForCourseAsync(id))
      throw new InvalidOperationException($"Can not delete course '{course.Name}' with enrollment");

    await _unitOfWork.Courses.RemoveAsync(course);
    _imageService.DeleteImage(course.ImagePath);
    await _unitOfWork.CompleteAsync();
  }

  public async Task<int> GetCoursesCountAsync()
  {
    return await _unitOfWork.Courses.CountAsync();
  }
}