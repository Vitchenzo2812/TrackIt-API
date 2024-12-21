using TrackIt.Entities.Expenses;

namespace TrackIt.Queries.GetPaymentFormats;

public record GetPaymentFormatsResult (
  Guid Id,
  string Title,
  PaymentFormatKey Key,
  string Icon,
  string IconColor,
  string BackgroundIconColor
)
{
  public static GetPaymentFormatsResult Build (GetPaymentFormatsRow row)
  {
    return new GetPaymentFormatsResult(
      Id: row.Id,
      Title: row.Title,
      Key: row.PaymentFormatKey,
      Icon: row.Icon,
      IconColor: row.IconColor,
      BackgroundIconColor: row.BackgroundIconColor
    );
  }
}