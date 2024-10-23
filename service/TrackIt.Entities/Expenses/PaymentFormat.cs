using TrackIt.Entities.Core;

namespace TrackIt.Entities.Expenses;

public class PaymentFormat : Aggregate
{
  public required PaymentFormatKey Key { get; set; }
  public required string Title { get; set; }
  
  public PaymentFormatConfig? PaymentFormatConfig { get; set; }

  public static PaymentFormat Create ()
  {
    return new PaymentFormat
    {
      Key = PaymentFormatKey.DEBIT_CARD,
      Title = string.Empty
    };
  }

  public PaymentFormat WithKey (PaymentFormatKey key)
  {
    Key = key;
    return this;
  }

  public PaymentFormat WithTitle (string title)
  {
    Title = title;
    return this;
  }
}