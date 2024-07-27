using TrackIt.Entities.Core;

namespace TrackIt.Infraestructure.Mailer.Models;

public class MailerForgotPasswordRequestData : Data
{
  public required string Code { get; set; }
}