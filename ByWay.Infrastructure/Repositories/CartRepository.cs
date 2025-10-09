using ByWay.Domain.Entities;
using ByWay.Domain.Interfaces.Repository;
using ByWay.Domain.Specifications;
using ByWay.Infrastructure.Data.Contexts.AppContext;
using ByWay.Infrastructure.Specifications.CartSpecifications;
using Microsoft.EntityFrameworkCore;

namespace ByWay.Infrastructure.Repositories
{
  public class CartRepository : Repository<Cart>, ICartRepository
  {
    public CartRepository(AppDbContext context) : base(context) { }



    public async Task<Cart> GetCartByUserIdAsync(string userId)
    {
      var spec = new CartWithItemsSpecification(userId);
      return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<CartItem> GetCartItemByCourseIdAsync(string userId, int courseId)
    {
      var spec = new CartWithItemsSpecification(userId);
      return await ApplySpecification(spec).AsNoTracking().SelectMany(c => c.Items).Where(item => item.CourseId == courseId).FirstOrDefaultAsync();
    }

    public async Task<bool> IsItemInCartAsync(string userId, int courseId)
    {
      var spec = new CartWithItemsSpecification(userId);
      return await ApplySpecification(spec).AsNoTracking().SelectMany(c => c.Items).Where(item => item.CourseId == courseId).AnyAsync();
    }
    public async Task AddCartItemAsync(CartItem item)
    {
      await _context.CartItems.AddAsync(item);
    }
    public Task RemoveCartItemAsync(CartItem item)
    {
      _context.CartItems.Remove(item);
      return Task.CompletedTask;
    }

    public override Task<int> CountAsync(ISpecification<Cart> spec)
    {
      return ApplySpecification(spec).SelectMany(c => c.Items).CountAsync();
    }
  }
}
