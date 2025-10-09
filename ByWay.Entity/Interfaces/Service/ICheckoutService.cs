namespace ByWay.Domain.Interfaces.Service;

public interface ICheckoutService
{
  Task ProcessCheckOutAsync(string userId);
}