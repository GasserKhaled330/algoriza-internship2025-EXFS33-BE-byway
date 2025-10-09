using ByWay.Domain.Entities;
using ByWay.Domain.Interfaces.Repository;
using ByWay.Infrastructure.Data.Contexts.AppContext;

namespace ByWay.Infrastructure.Repositories;

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
  public PaymentRepository(AppDbContext context) : base(context)
  {

  }
}