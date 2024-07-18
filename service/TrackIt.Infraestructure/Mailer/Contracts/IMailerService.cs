using TrackIt.Infraestructure.Mailer.Models;

namespace TrackIt.Infraestructure.Mailer.Contracts;

public interface IMailerService
{
  Task Send (MailRequest request);
}