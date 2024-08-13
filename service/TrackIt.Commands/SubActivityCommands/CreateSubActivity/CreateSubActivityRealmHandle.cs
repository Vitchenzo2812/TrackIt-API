using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.SubActivityCommands.CreateSubActivity;

public class CreateSubActivityRealmHandle : IPipelineBehavior<CreateSubActivityCommand, Unit>
{
  private readonly IUserRepository _userRepository;

  private readonly IActivityRepository _activityRepository;

  private readonly IActivityGroupRepository _activityGroupRepository;

  public CreateSubActivityRealmHandle (
    IUserRepository userRepository,
    IActivityRepository activityRepository,
    IActivityGroupRepository activityGroupRepository
  )
  {
    _userRepository = userRepository;
    _activityRepository = activityRepository;
    _activityGroupRepository = activityGroupRepository;
  }
  
  public async Task<Unit> Handle (CreateSubActivityCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();
      
    var activityGroup = await _activityGroupRepository.FindById(request.Aggregate.ActivityGroupId);

    if (activityGroup is null)
      throw new NotFoundError("Activity group not found");
    
    if (await _activityRepository.FindById(request.Aggregate.ActivityId) is null)
      throw new NotFoundError("Activity not found");
    
    if (request.Session.Id == activityGroup.UserId)
      return await next();
    
    throw new ForbiddenError();
  }
}