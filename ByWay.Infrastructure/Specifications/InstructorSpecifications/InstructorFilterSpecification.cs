using ByWay.Domain.Entities;
using ByWay.Domain.Enums;

namespace ByWay.Application.Specifications.InstructorSpecifications;

public class InstructorFilterSpecification : BaseSpecification<Instructor>
{
  public InstructorFilterSpecification(string? name = null, JobTitle? jobTitle = null) :
      base(i =>
          (string.IsNullOrEmpty(name) ||
          i.FullName.ToLower().Contains(name.ToLower()))
          &&
          (!jobTitle.HasValue || i.JobTitle == jobTitle.Value)
      )
  {
  }
}
