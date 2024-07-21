using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.EventBus.Contracts;
using TrackIt.Infraestructure.Mailer.Contracts;
using TrackIt.Infraestructure.Mailer.Models;
using TrackIt.Entities.Events;
using MassTransit;

namespace TrackIt.Events.Consumers;

public class SendEmailAboutSignUpConsumer : IEventConsumer<SendEmailVerificationEvent>
{
  private readonly IUserRepository _userRepository;
  
  private readonly IMailerService _mailerService;

  public SendEmailAboutSignUpConsumer (
    IMailerService mailerService,
    IUserRepository userRepository
  )
  {
    _userRepository = userRepository;
    _mailerService = mailerService;
  }
  
  public async Task Consume (ConsumeContext<SendEmailVerificationEvent> @event)
  {
    MailRequest request = new MailRequest(
      Addresses: [@event.Message.ValidationObject],
      
      Template: "",
      
      Subject: "Verificação de Email",
      
      Data: new MailerSignUpRequestData { Code = @event.Message.Code }
    );

    await _mailerService.Send(request);
  }
}