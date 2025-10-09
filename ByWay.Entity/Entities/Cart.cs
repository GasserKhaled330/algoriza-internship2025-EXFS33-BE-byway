namespace ByWay.Domain.Entities
{
  public class Cart : BaseEntity
  {
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }


    public ICollection<CartItem> Items { get; set; } = [];
  }
}
