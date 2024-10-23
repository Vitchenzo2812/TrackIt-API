using TrackIt.Queries.Views.HomePage;
using TrackIt.Entities.Repository;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Queries.GetHomePageInfo;

public class GetHomePageInfoRealmHandle : IPipelineBehavior<GetHomePageInfoQuery, HomePageView>
{
  private readonly IUserRepository _userRepository;
  private readonly IActivityGroupRepository _activityGroupRepository;

  public GetHomePageInfoRealmHandle (
    IUserRepository userRepository,
    IActivityGroupRepository activityGroupRepository
  )
  {
    _userRepository = userRepository;
    _activityGroupRepository = activityGroupRepository;
  }
  
  public async Task<HomePageView> Handle (GetHomePageInfoQuery request, RequestHandlerDelegate<HomePageView> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();
    
    if (await _activityGroupRepository.FindById(request.Params.ActivityGroupId) is null)
      throw new NotFoundError("Activity group not found");
    
    return await next();
  }
}