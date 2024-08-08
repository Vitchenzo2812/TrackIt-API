using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.ActivityCommands.CreateActivity;

public class CreateActivityRealmHandle : IPipelineBehavior<CreateActivityCommand, Unit>
{
  private readonly IUserRepository _userRepository;
  
  private readonly IActivityGroupRepository _activityGroupRepository;

  public CreateActivityRealmHandle (
    IUserRepository userRepository,
    IActivityGroupRepository activityGroupRepository
  )
  {
    _userRepository = userRepository;
    _activityGroupRepository = activityGroupRepository;
  }
  
  public async Task<Unit> Handle (CreateActivityCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);
    
    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();

    if (await _activityGroupRepository.FindById(request.AggregateId) is null)
      throw new NotFoundError("Activity group not found");

    return await next();
  }
}