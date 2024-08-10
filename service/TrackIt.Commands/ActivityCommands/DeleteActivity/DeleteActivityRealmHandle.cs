using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.ActivityCommands.DeleteActivity;

public class DeleteActivityRealmHandle : IPipelineBehavior<DeleteActivityCommand, Unit>
{
  private readonly IUserRepository _userRepository;
  
  private readonly IActivityRepository _activityRepository;
  
  private readonly IActivityGroupRepository _activityGroupRepository;

  public DeleteActivityRealmHandle (
    IUserRepository userRepository,
    IActivityRepository activityRepository,
    IActivityGroupRepository activityGroupRepository
  )
  {
    _userRepository = userRepository;
    _activityRepository = activityRepository;
    _activityGroupRepository = activityGroupRepository;
  }
  
  public async Task<Unit> Handle (DeleteActivityCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);
    
    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();

    if (await _activityGroupRepository.FindById(request.Aggregate.Id) is null)
      throw new NotFoundError("Activity group not found");

    if (await _activityRepository.FindById(request.Aggregate.EntityId) is null)
      throw new NotFoundError("Activity not found");
    
    return await next();
  }
}