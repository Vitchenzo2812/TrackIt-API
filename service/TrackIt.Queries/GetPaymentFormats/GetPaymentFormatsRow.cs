using TrackIt.Entities.Expenses;

namespace TrackIt.Queries.GetPaymentFormats;

public record GetPaymentFormatsRow (
  Guid Id,
  string Title,
  PaymentFormatKey PaymentFormatKey,
  string Icon,
  string IconColor,
  string BackgroundIconColor
);