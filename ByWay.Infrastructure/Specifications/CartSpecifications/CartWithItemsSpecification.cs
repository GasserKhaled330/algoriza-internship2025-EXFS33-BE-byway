namespace ByWay.Infrastructure.Specifications.CartSpecifications
{
  public class CartWithItemsSpecification : CartSpecification
  {
    public CartWithItemsSpecification(string userId) :
      base(c => c.AppUserId == userId)
    {

    }
  }
}
