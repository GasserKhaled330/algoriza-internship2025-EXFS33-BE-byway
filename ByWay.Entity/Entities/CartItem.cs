namespace ByWay.Domain.Entities
{
  public class CartItem : BaseEntity
  {
    public int CartId { get; set; }
    public Cart Cart { get; set; }
    public int CourseId { get; set; }
  }
}
