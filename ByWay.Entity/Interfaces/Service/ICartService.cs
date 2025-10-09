using ByWay.Domain.Dtos.CartDtos;

namespace ByWay.Domain.Interfaces.Service;

public interface ICartService
{
  Task AddItemAsync(string userId, int courseId);
  Task RemoveItemAsync(string userId, int courseId);
  Task<List<CartItemDto>> GetCartAsync(string userId);
  Task ClearCartAsync(string userId);
  Task<int> GetCartItemsCount(string userId);
  Task<bool> IsInCart(string userId, int courseId);
}
