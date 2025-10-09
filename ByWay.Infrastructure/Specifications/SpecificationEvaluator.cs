using ByWay.Domain.Entities;
using ByWay.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace ByWay.Application.Specifications;

public class SpecificationEvaluator<T> where T : BaseEntity
{
  public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
  {
    var query = inputQuery;

    if (spec.Criteria is not null)
      query = query.Where(spec.Criteria);

    if (spec.OrderBy != null)
      query = query.OrderBy(spec.OrderBy);

    if (spec.OrderByDescending != null)
      query = query.OrderByDescending(spec.OrderByDescending);

    if (spec is { Skip: not null, Take: not null })
      query = query.Skip(spec.Skip.Value).Take(spec.Take.Value);

    query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

    return query;
  }
}