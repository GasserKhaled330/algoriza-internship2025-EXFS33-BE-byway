using ByWay.Domain.DTOs;
using ByWay.Domain.Entities;
using ByWay.Domain.Enums;

namespace ByWay.Domain.Interfaces.Service;

public interface IInstructorService
{
  Task<(IReadOnlyList<Instructor>, int TotalCount)> GetPagedInstructorsAsync(int pageIndex, int pageSize, string? searchTerm = null, JobTitle? jobTitle = null);
  Task<Instructor?> GetInstructorByIdAsync(int id);
  string[] GetInstructorJobTitles();
  Task<int> GetInstructorsCountAsync();
  Task<Instructor> AddInstructorAsync(AddInstructorDto instructor);
  Task UpdateInstructorAsync(int id, UpdateInstructorDto updateInstructorDto);
  Task RemoveInstructorAsync(int id);
}
