using ByWay.Domain.Entities;

namespace ByWay.Domain.Interfaces.Repository
{
  public interface ICartRepository : IRepository<Cart>
  {
    Task<Cart> GetCartByUserIdAsync(string userId);
    Task<CartItem> GetCartItemByCourseIdAsync(string userId, int courseId);
    Task<bool> IsItemInCartAsync(string userId, int courseId);
    Task AddCartItemAsync(CartItem item);
    Task RemoveCartItemAsync(CartItem item);
  }
}
