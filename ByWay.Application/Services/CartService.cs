using ByWay.Domain.Dtos.CartDtos;
using ByWay.Domain.Entities;
using ByWay.Domain.Interfaces.Service;
using ByWay.Domain.Interfaces.UnitOfWork;
using ByWay.Infrastructure.Specifications.CartSpecifications;
using Microsoft.AspNetCore.Http;

namespace ByWay.Application.Services;

public class CartService : ICartService
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly ICourseService _courseService;
  private readonly IUnitOfWork _unitOfWork;
  public CartService(ICourseService courseService,
      IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
  {
    _courseService = courseService;
    //_redisDb = redis.GetDatabase();
    _httpContextAccessor = httpContextAccessor;
    _unitOfWork = unitOfWork;
  }

  public async Task AddItemAsync(string userId, int courseId)
  {
    var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);

    if (cart is null)
    {
      cart = new Cart { AppUserId = userId };
      await _unitOfWork.Carts.AddAsync(cart);
      await _unitOfWork.CompleteAsync();
    }

    var existingItem = await _unitOfWork.Carts.GetCartItemByCourseIdAsync(userId, courseId);

    if (existingItem is null)
    {
      var course = await _courseService.GetCourseByIdAsync(courseId);

      //var request = _httpContextAccessor.HttpContext?.Request;
      //var baseUrl = $"{request?.Scheme}://{request?.Host}{request?.PathBase}";
      //var imagePath = $"{baseUrl}/{(course.ImagePath.StartsWith('/') ? course.ImagePath[1..] : course.ImagePath)}";
      var newItem = new CartItem
      {
        CartId = cart.Id,
        CourseId = courseId,
      };

      await _unitOfWork.Carts.AddCartItemAsync(newItem);
      await _unitOfWork.CompleteAsync();
    }
  }

  public async Task<List<CartItemDto>> GetCartAsync(string userId)
  {
    var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
    if (cart is null || cart.Items.Count <= 0)
    {
      return [];
    }
    var cartItems = new List<CartItemDto>();

    foreach (var item in cart.Items)
    {
      var course = await _courseService.GetCourseByIdAsync(item.CourseId);
      if (course == null) continue;

      var request = _httpContextAccessor.HttpContext?.Request;
      var baseUrl = $"{request?.Scheme}://{request?.Host}{request?.PathBase}";
      var imagePath = $"{baseUrl}/{(course.ImagePath.StartsWith('/') ? course.ImagePath[1..] : course.ImagePath)}";

      cartItems.Add(new CartItemDto(
          CourseId: course.Id,
          Name: course.Name,
          InstructorName: course.Instructor.FullName,
          Rate: course.Rate,
          Level: Enum.GetName(course.Level),
          TotalHours: course.TotalHours,
          LecturesCount: course.Contents.Select(content => content.LecturesCount).Sum(),
          Cost: course.Cost,
          ImagePath: imagePath
      ));
    }
    return cartItems;
  }

  public async Task RemoveItemAsync(string userId, int courseId)
  {
    var cartItem = await _unitOfWork.Carts.GetCartItemByCourseIdAsync(userId, courseId);
    if (cartItem is not null)
    {
      await _unitOfWork.Carts.RemoveCartItemAsync(cartItem);
      await _unitOfWork.CompleteAsync();
    }
  }

  public async Task ClearCartAsync(string userId)
  {
    var cart = await _unitOfWork.Carts.GetCartByUserIdAsync(userId);
    if (cart is not null)
    {
      _unitOfWork.Carts.RemoveAsync(cart);
      await _unitOfWork.CompleteAsync();
    }
  }

  public async Task<int> GetCartItemsCount(string userId)
  {
    var spec = new CartWithItemsSpecification(userId);
    return await _unitOfWork.Carts.CountAsync(spec);
  }

  public async Task<bool> IsInCart(string userId, int courseId)
  {
    return await _unitOfWork.Carts.IsItemInCartAsync(userId, courseId);
  }
}