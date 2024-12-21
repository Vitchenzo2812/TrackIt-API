using TrackIt.Entities.Expenses;
using TrackIt.Queries.GetPaymentFormats;

namespace TrackIt.Tests.Mocks.Entities;

public class PaymentFormatsMock : PaymentFormat
{
  public static void Verify (GetPaymentFormatsResult expect, PaymentFormat current, PaymentFormatConfig config)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.Title, current.Title);
    Assert.Equal(expect.Key, current.Key);
    Assert.Equal(expect.Icon, config.Icon);
    Assert.Equal(expect.IconColor, config.IconColor);
    Assert.Equal(expect.BackgroundIconColor, config.BackgroundIconColor);
  }  
}