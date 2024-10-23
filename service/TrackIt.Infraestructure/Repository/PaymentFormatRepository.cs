using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Expenses;

namespace TrackIt.Infraestructure.Repository;

public class PaymentFormatRepository : IPaymentFormatRepository
{
  private readonly TrackItDbContext _db;

  public PaymentFormatRepository (TrackItDbContext db) => _db = db;
  
  public async Task<PaymentFormat?> FindById (Guid aggregateId)
  {
    return await _db.PaymentFormats
      .AsTracking()
      .Include(x => x.PaymentFormatConfig)
      .FirstOrDefaultAsync(x => x.Id == aggregateId);
  }

  public void Save (PaymentFormat aggregate)
  {
    _db.PaymentFormats.Add(aggregate);
  }

  public void Delete (PaymentFormat aggregate)
  {
    _db.PaymentFormats.Remove(aggregate);
  }
}