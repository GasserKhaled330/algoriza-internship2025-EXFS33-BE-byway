using ByWay.Domain.Entities;
using ByWay.Domain.Specifications;

namespace ByWay.Domain.Interfaces.Repository;

public interface IRepository<T> where T : BaseEntity
{
  Task<T?> GetBySpecAsync(ISpecification<T> spec);
  Task<IReadOnlyList<T>> GetAllBySpecAsync(ISpecification<T> spec);
  Task AddAsync(T entity);
  Task UpdateAsync(T entity);
  Task RemoveAsync(T entity);
  Task<int> CountAsync(ISpecification<T> spec);
  Task<int> CountAsync();
}