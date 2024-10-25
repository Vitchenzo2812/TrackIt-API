using TrackIt.Entities.Repository;
using TrackIt.Entities.Activities;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Queries.GetActivities;

public class GetActivitiesRealmHandle : IPipelineBehavior<GetActivitiesQuery, List<Activity>>
{
  private readonly IUserRepository _userRepository;

  public GetActivitiesRealmHandle (
    IUserRepository userRepository
  )
  {
    _userRepository = userRepository;
  }
  
  public async Task<List<Activity>> Handle (GetActivitiesQuery request, RequestHandlerDelegate<List<Activity>> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();
    
    return await next();
  }
}