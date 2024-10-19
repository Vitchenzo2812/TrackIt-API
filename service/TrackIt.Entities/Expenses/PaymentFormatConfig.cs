using TrackIt.Entities.Core;

namespace TrackIt.Entities.Expenses;

public class PaymentFormatConfig : Entity
{
  public required string Icon { get; set; }
  public required string IconColor { get; set; }
  public required string BackgroundIconColor { get; set; }
}