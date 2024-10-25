using TrackIt.Entities.Repository;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Queries.GetActivityGroups;

public class GetActivityGroupsRealmHandle : IPipelineBehavior<GetActivityGroupsQuery, List<GetActivityGroupsResult>>
{
  private readonly IUserRepository _userRepository;

  public GetActivityGroupsRealmHandle (
    IUserRepository userRepository
  )
  {
    _userRepository = userRepository;
  }
  
  public async Task<List<GetActivityGroupsResult>> Handle (GetActivityGroupsQuery request, RequestHandlerDelegate<List<GetActivityGroupsResult>> next, CancellationToken cancellationToken)
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