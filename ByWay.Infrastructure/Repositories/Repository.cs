using ByWay.Application.Specifications;
using ByWay.Domain.Entities;
using ByWay.Domain.Interfaces.Repository;
using ByWay.Domain.Specifications;
using ByWay.Infrastructure.Data.Contexts.AppContext;
using Microsoft.EntityFrameworkCore;

namespace ByWay.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{

  protected readonly AppDbContext _context;
  protected readonly DbSet<T> _dbSet;

  public Repository(AppDbContext context)
  {
    _context = context;
    _dbSet = _context.Set<T>();
  }

  public async Task<T?> GetBySpecAsync(ISpecification<T> spec)
  {
    return await ApplySpecification(spec).FirstOrDefaultAsync();
  }

  public async Task<IReadOnlyList<T>> GetAllBySpecAsync(ISpecification<T> spec)
  {
    return await ApplySpecification(spec).ToListAsync();
  }

  public virtual async Task AddAsync(T entity)
  {
    await _dbSet.AddAsync(entity);
  }

  public virtual Task UpdateAsync(T entity)
  {
    _dbSet.Update(entity);
    return Task.CompletedTask;
  }

  public virtual Task RemoveAsync(T entity)
  {
    _dbSet.Remove(entity);
    return Task.CompletedTask;
  }

  public virtual async Task<int> CountAsync(ISpecification<T> spec)
  {
    return await ApplySpecification(spec).CountAsync();
  }

  public virtual async Task<int> CountAsync()
  {
    return await _dbSet.CountAsync();
  }

  protected IQueryable<T> ApplySpecification(ISpecification<T> spec)
  {
    return SpecificationEvaluator<T>.GetQuery(_dbSet.AsQueryable(), spec);
  }
}