using ByWay.Domain.Enums;

namespace ByWay.Application.Specifications.InstructorSpecifications;

public class PagedInstructorSpecification : InstructorFilterSpecification
{
  public PagedInstructorSpecification(int pageIndex, int pageSize, string? name = null, JobTitle? jobTitle = null)
      : base(name, jobTitle)
  {
    ApplyPaging((pageIndex - 1) * pageSize, pageSize);
    AddOrderByDescending(i => i.Rate);
  }
}
