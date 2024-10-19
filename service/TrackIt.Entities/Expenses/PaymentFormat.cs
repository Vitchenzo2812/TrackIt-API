using TrackIt.Entities.Core;

namespace TrackIt.Entities.Expenses;

public class PaymentFormat : Aggregate
{
  public required PaymentFormatKey Key { get; set; }
  public required string Title { get; set; }
}