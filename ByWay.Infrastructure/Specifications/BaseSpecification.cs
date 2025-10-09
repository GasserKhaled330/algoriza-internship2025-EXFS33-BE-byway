using ByWay.Domain.Specifications;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace ByWay.Application.Specifications;

public class BaseSpecification<T> : ISpecification<T>
{
  public Expression<Func<T, bool>>? Criteria { get; private set; }
  public Collection<Expression<Func<T, object>>> Includes { get; } = new();
  public Expression<Func<T, object>>? OrderBy { get; private set; }
  public Expression<Func<T, object>>? OrderByDescending { get; private set; }
  public int? Take { get; private set; }
  public int? Skip { get; private set; }

  protected BaseSpecification()
  {
  }

  protected BaseSpecification(Expression<Func<T, bool>> criteria)
  {
    Criteria = criteria;
  }

  protected void AddInclude(Expression<Func<T, object>> includeExpression)
  {
    Includes.Add(includeExpression);
  }
  protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
  {
    OrderBy = orderByExpression;
  }

  protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
  {
    OrderByDescending = orderByDescExpression;
  }

  protected void ApplyPaging(int skip, int take)
  {
    Skip = skip;
    Take = take;
  }
}