using ByWay.Application.Specifications.InstructorSpecifications;
using ByWay.Domain.DTOs;
using ByWay.Domain.Entities;
using ByWay.Domain.Enums;
using ByWay.Domain.Interfaces.Service;
using ByWay.Domain.Interfaces.UnitOfWork;
using ByWay.Infrastructure.Specifications.InstructorSpecifications;

namespace ByWay.Application.Services;

public class InstructorService : IInstructorService
{
  private const string InstructorImagesFolder = "images/courses/";
  private readonly IUnitOfWork _unitOfWork;
  private readonly IImageService _imageService;

  public InstructorService(IUnitOfWork unitOfWork, IImageService imageService)
  {
    _unitOfWork = unitOfWork;
    _imageService = imageService;
  }

  public async Task<(IReadOnlyList<Instructor>, int TotalCount)> GetPagedInstructorsAsync(int pageIndex, int pageSize,
      string? name = null, JobTitle? jobTitle = null)
  {
    var countSpec = new InstructorFilterSpecification(name, jobTitle);
    var spec = new PagedInstructorSpecification(pageIndex, pageSize, name, jobTitle);

    var count = await _unitOfWork.Instructors.CountAsync(countSpec);
    var instructors = await _unitOfWork.Instructors.GetAllBySpecAsync(spec);


    return (instructors, count);
  }

  public async Task<Instructor?> GetInstructorByIdAsync(int id)
  {
    var spec = new InstructorSpecification(id);
    return await _unitOfWork.Instructors.GetBySpecAsync(spec);
  }

  public async Task<Instructor> AddInstructorAsync(AddInstructorDto addInstructorDto)
  {
    var imagePath = await _imageService.UploadImageAsync(
        addInstructorDto.Image,
        InstructorImagesFolder
    );

    var instructor = new Instructor
    {
      FullName = addInstructorDto.FullName,
      Bio = addInstructorDto.Bio,
      Rate = addInstructorDto.Rate,
      JobTitle = addInstructorDto.JobTitle,
      ImagePath = imagePath
    };

    await _unitOfWork.Instructors.AddAsync(instructor);
    await _unitOfWork.CompleteAsync();
    return instructor;
  }

  public async Task UpdateInstructorAsync(int id, UpdateInstructorDto updateInstructorDto)
  {
    var spec = new InstructorSpecification(id);
    var instructor = await _unitOfWork.Instructors.GetBySpecAsync(spec) ??
                     throw new KeyNotFoundException($"Instructor with id {id} not found.");

    if (updateInstructorDto.FullName is not null)
      instructor.FullName = updateInstructorDto.FullName;
    if (updateInstructorDto.Bio is not null)
      instructor.Bio = updateInstructorDto.Bio;
    if (updateInstructorDto.Rate.HasValue)
      instructor.Rate = updateInstructorDto.Rate.Value;
    if (updateInstructorDto.JobTitle.HasValue)
      instructor.JobTitle = updateInstructorDto.JobTitle.Value;

    if (updateInstructorDto.Image is not null && updateInstructorDto.Image.Length > 0)
    {
      var newImagePath = await _imageService.UploadImageAsync(
          updateInstructorDto.Image,
          InstructorImagesFolder
      );

      _imageService.DeleteImage(instructor.ImagePath);
      instructor.ImagePath = newImagePath;
    }

    await _unitOfWork.CompleteAsync();
  }

  public async Task RemoveInstructorAsync(int id)
  {
    var spec = new InstructorSpecification(id);
    var instructor = await _unitOfWork.Instructors.GetBySpecAsync(spec) ??
                     throw new KeyNotFoundException($"Instructor with id {id} not found.");

    if (await _unitOfWork.Instructors.HasCoursesAsync((instructor.Id)))
      throw new InvalidOperationException(
          $"Cannot delete instructor '{instructor.FullName}' with assigned courses.");

    await _unitOfWork.Instructors.RemoveAsync(instructor);
    _imageService.DeleteImage(instructor.ImagePath);
    await _unitOfWork.CompleteAsync();
  }

  public string[] GetInstructorJobTitles()
  {
    return Enum.GetNames(typeof(JobTitle));
  }

  public async Task<int> GetInstructorsCountAsync()
  {
    return await _unitOfWork.Instructors.CountAsync();
  }
}