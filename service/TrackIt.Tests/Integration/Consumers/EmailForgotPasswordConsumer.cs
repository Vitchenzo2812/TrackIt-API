using TrackIt.Infraestructure.Mailer.Models;
using TrackIt.Entities.Events;
using TrackIt.Tests.Config;
using Moq;

namespace TrackIt.Tests.Integration.Consumers;

public class EmailForgotPasswordConsumer (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldSendEmail ()
  {
    await _harness.Bus.Publish(
      new ForgotPasswordEvent("gvitchenzo@gmail.com", "123456")
    );
    
    var consumed = await _harness.Consumed.Any<ForgotPasswordEvent>();
    
    Assert.True(consumed);
    _factory.MailerServiceMock.Verify(m => m.Send(It.IsAny<MailRequest>()), Times.Once);
  }
}