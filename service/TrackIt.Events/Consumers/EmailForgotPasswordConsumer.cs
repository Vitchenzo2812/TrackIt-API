using TrackIt.Infraestructure.EventBus.Contracts;
using TrackIt.Infraestructure.Mailer.Contracts;
using TrackIt.Infraestructure.Mailer.Models;
using TrackIt.Entities.Events;
using MassTransit;

namespace TrackIt.Events.Consumers;

public class EmailForgotPasswordConsumer : IEventConsumer<ForgotPasswordEvent>
{
  private readonly IMailerService _mailerService;

  public EmailForgotPasswordConsumer (IMailerService mailerService)
  {
    _mailerService = mailerService;
  }
  
  public async Task Consume (ConsumeContext<ForgotPasswordEvent> @event)
  {
    MailRequest request = new MailRequest(
      Addresses: [@event.Message.ValidationObject],
      
      Template: "forgot_password.html",
      
      Subject: "Atualização de Senha",
      
      Data: new MailerForgotPasswordRequestData { Code = @event.Message.Code }
    );

    await _mailerService.Send(request);
  }
}