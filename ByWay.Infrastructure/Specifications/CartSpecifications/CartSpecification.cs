using ByWay.Application.Specifications;
using ByWay.Domain.Entities;
using System.Linq.Expressions;

namespace ByWay.Infrastructure.Specifications.CartSpecifications
{
  public class CartSpecification : BaseSpecification<Cart>
  {
    public CartSpecification(Expression<Func<Cart, bool>> criteria) : base(criteria)
    {
      AddInclude(c => c.Items);
    }
  }
}
