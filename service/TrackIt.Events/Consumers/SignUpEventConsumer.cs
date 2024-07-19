using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.EventBus.Contracts;
using TrackIt.Infraestructure.Mailer.Contracts;
using TrackIt.Infraestructure.Mailer.Models;
using TrackIt.Entities.Errors;
using TrackIt.Entities.Events;
using MassTransit;

namespace TrackIt.Events.Consumers;

public class SignUpEventConsumer : IEventConsumer<SignUpEvent>
{
  private readonly IUserRepository _userRepository;
  
  private readonly IMailerService _mailerService;

  public SignUpEventConsumer (
    IMailerService mailerService,
    IUserRepository userRepository
  )
  {
    _userRepository = userRepository;
    _mailerService = mailerService;
  }
  
  public async Task Consume (ConsumeContext<SignUpEvent> @event)
  {
    var user = await _userRepository.FindById(@event.Message.UserId);

    if (user?.Email is null)
      throw new NotFoundError("User not found");

    MailRequest request = new MailRequest(
      Addresses: [user.Email.Value],
      
      Template: "",
      
      Subject: "Verificação de Email",
      
      Data: new MailerSignUpRequestData {}
    );

    await _mailerService.Send(request);
  }
}