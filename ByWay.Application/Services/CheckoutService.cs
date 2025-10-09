using ByWay.Application.Exceptions;
using ByWay.Domain.Entities;
using ByWay.Domain.Interfaces.Service;
using ByWay.Domain.Interfaces.UnitOfWork;

namespace ByWay.Application.Services;

public class CheckoutService : ICheckoutService
{
  private readonly ICourseService _courseService;
  private readonly ICartService _cartService;
  private readonly IUnitOfWork _unitOfWork;

  public CheckoutService(ICourseService courseService, ICartService cartService, IUnitOfWork unitOfWork)
  {
    _courseService = courseService;
    _cartService = cartService;
    _unitOfWork = unitOfWork;
  }

  public async Task ProcessCheckOutAsync(string userId)
  {
    var cartItems = await _cartService.GetCartAsync(userId);
    if (cartItems == null || cartItems.Count == 0)
    {
      throw new CheckoutValidationException("Cart is empty or could not be loaded.");
    }

    decimal totalCost = 0;

    foreach (var cartItem in cartItems)
    {
      var alreadyEnrolled = await _unitOfWork.Enrollments.IsEnrolledAsync(userId, cartItem.CourseId);
      if (alreadyEnrolled)
      {
        throw new CheckoutValidationException(
            $"User is already enrolled in course {cartItem.Name} ({cartItem.CourseId}).");
      }

      totalCost += cartItem.Cost;
    }

    try
    {
      var payment = new Payment
      {
        Amount = totalCost,
        AppUserId = userId
      };
      await _unitOfWork.Payments.AddAsync(payment);

      foreach (var enrollment in cartItems.Select(cartItem => new Enrollment
      {
        CourseId = cartItem.CourseId,
        AppUserId = userId,
        Payment = payment
      }))
      {
        await _unitOfWork.Enrollments.AddAsync(enrollment);
      }

      await _unitOfWork.CompleteAsync();

      await _cartService.ClearCartAsync(userId);
    }
    catch (CheckoutValidationException)
    {
      throw;
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Critical Checkout Error for user {userId}: {ex.Message}");
      throw;
    }
  }
}
