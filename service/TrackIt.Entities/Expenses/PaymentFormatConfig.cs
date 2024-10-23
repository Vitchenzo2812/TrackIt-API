using TrackIt.Entities.Core;

namespace TrackIt.Entities.Expenses;

public class PaymentFormatConfig : Entity
{
  public Guid? PaymentFormatId { get; set; }
  public PaymentFormat? PaymentFormat { get; set; }
  
  public required string Icon { get; set; }
  public required string IconColor { get; set; }
  public required string BackgroundIconColor { get; set; }

  public static PaymentFormatConfig Create ()
  {
    return new PaymentFormatConfig
    {
      Icon = string.Empty,
      IconColor = string.Empty,
      BackgroundIconColor = string.Empty
    };
  }

  public PaymentFormatConfig WithIcon (string icon)
  {
    Icon = icon;
    return this;
  }
  
  public PaymentFormatConfig WithIconColor (string iconColor)
  {
    IconColor = iconColor;
    return this;
  }
  
  public PaymentFormatConfig WithBackgroundIconColor (string backgroundIconColor)
  {
    BackgroundIconColor = backgroundIconColor;
    return this;
  }

  public PaymentFormatConfig AssignToPaymentFormat (Guid paymentFormatId)
  {
    PaymentFormatId = paymentFormatId;
    return this;
  }
}