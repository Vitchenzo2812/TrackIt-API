using TrackIt.Infraestructure.Mailer.Contracts;
using TrackIt.Infraestructure.Mailer.Models;

namespace TrackIt.Tests.Mocks.Infra;

public class MailerServiceMock : IMailerService
{
  public Task Send (MailRequest request)
  {
    return Task.CompletedTask;
  }
}