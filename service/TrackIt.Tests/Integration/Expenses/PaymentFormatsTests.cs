using TrackIt.Infraestructure.Extensions;
using TrackIt.Queries.GetPaymentFormats;
using TrackIt.Tests.Config.Builders;
using TrackIt.Tests.Mocks.Entities;
using TrackIt.Entities.Expenses;
using TrackIt.Tests.Config;
using System.Net;

namespace TrackIt.Tests.Integration.Expenses;

public class PaymentFormatsTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  private PaymentFormat paymentFormat1 { get; set; }
  private PaymentFormat paymentFormat2 { get; set; }
  private PaymentFormatConfig paymentFormatConfig1 { get; set; }
  private PaymentFormatConfig paymentFormatConfig2 { get; set; }
  
  [Fact]
  public async Task ShouldGetPaymentFormats ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreatePaymentFormats();

    var response = await _httpClient.GetAsync("payment-format");
    var result = await response.ToData<List<GetPaymentFormatsResult>>();
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    Assert.Equal(8, result.Count);

    List<PaymentFormat> paymentFormats = [paymentFormat1, paymentFormat2];
    List<PaymentFormatConfig> paymentConfigs = [paymentFormatConfig1, paymentFormatConfig2];

    foreach (var expect in result)
    {
      var payment = paymentFormats.Find(x => x.Id == expect.Id);
      var config = paymentConfigs.Find(x => x.PaymentFormat?.Id == expect.Id);

      if (payment is null || config is null)
        continue;
      
      PaymentFormatsMock.Verify(expect, payment, config);
    }
  }
  
  #region setup for tests

    private async Task CreatePaymentFormats ()
    {
      paymentFormat1 = PaymentFormat.Create()
        .WithTitle("PIX")
        .WithKey(PaymentFormatKey.PIX);
      
      paymentFormatConfig1 = PaymentFormatConfig.Create()
        .AssignToPaymentFormat(paymentFormat1.Id)
        .WithIcon("ICON_1")
        .WithIconColor("ICON_COLOR_1")
        .WithBackgroundIconColor("BACKGROUND_ICON_COLOR_1");
      
      paymentFormat2 = PaymentFormat.Create()
        .WithTitle("Dinheiro")
        .WithKey(PaymentFormatKey.MONEY);
      
      paymentFormatConfig2 = PaymentFormatConfig.Create()
        .AssignToPaymentFormat(paymentFormat2.Id)
        .WithIcon("ICON_2")
        .WithIconColor("ICON_COLOR_2")
        .WithBackgroundIconColor("BACKGROUND_ICON_COLOR_2");
      
      _db.PaymentFormats.AddRange([paymentFormat1, paymentFormat2]);
      _db.PaymentFormatConfigs.AddRange([paymentFormatConfig1, paymentFormatConfig2]);

      await _db.SaveChangesAsync();
    } 

  #endregion
}