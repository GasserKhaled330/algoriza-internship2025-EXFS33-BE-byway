using ByWay.Application.Specifications;
using ByWay.Domain.Entities;

namespace ByWay.Infrastructure.Specifications.InstructorSpecifications
{
  public class InstructorSpecification : BaseSpecification<Instructor>
  {
    public InstructorSpecification(int id) : base(
      i => i.Id == id
      )
    { }
  }
}
