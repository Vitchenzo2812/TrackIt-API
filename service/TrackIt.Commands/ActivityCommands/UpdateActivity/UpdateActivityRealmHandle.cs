using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.ActivityCommands.UpdateActivity;

public class UpdateActivityRealmHandle : IPipelineBehavior<UpdateActivityCommand, Unit>
{
  private readonly IUserRepository _userRepository;
  
  private readonly IActivityRepository _activityRepository;
  
  private readonly IActivityGroupRepository _activityGroupRepository;

  public UpdateActivityRealmHandle (
    IUserRepository userRepository,
    IActivityRepository activityRepository,
    IActivityGroupRepository activityGroupRepository
  )
  {
    _userRepository = userRepository;
    _activityRepository = activityRepository;
    _activityGroupRepository = activityGroupRepository;
  }
  
  public async Task<Unit> Handle (UpdateActivityCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
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

    if (await _activityRepository.FindById(request.Payload.ActivityId) is null)
      throw new NotFoundError("Activity not found");
    
    return await next();
  }
}