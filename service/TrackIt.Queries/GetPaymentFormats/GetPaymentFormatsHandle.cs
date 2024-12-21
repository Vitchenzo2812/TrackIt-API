using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Queries.GetPaymentFormats;

public class GetPaymentFormatsHandle : IRequestHandler<GetPaymentFormatsQuery, List<GetPaymentFormatsResult>>
{
  private readonly TrackItDbContext _db;

  public GetPaymentFormatsHandle (TrackItDbContext db) => _db = db;
  
  public async Task<List<GetPaymentFormatsResult>> Handle (GetPaymentFormatsQuery request, CancellationToken cancellationToken)
  {
    var sql = await _db.Database
      .SqlQueryRaw<GetPaymentFormatsRow>(
        $"""
            SELECT
                payment.Id AS Id,
                payment.Title AS Title,
                payment.Key AS PaymentFormatKey,
                config.Icon AS Icon,
                config.IconColor AS IconColor,
                config.BackgroundIconColor AS BackgroundIconColor
            FROM PaymentFormats payment
            LEFT JOIN PaymentFormatConfigs config ON config.PaymentFormatId = payment.Id
         """
      )
      .ToListAsync();

    if (sql is null)
      throw new NotFoundError("Payment Method not found");

    return sql.Select(GetPaymentFormatsResult.Build).ToList();
  }
}