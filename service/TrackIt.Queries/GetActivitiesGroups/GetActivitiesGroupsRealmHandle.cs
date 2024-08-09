using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using TrackIt.Queries.Views;
using MediatR;

namespace TrackIt.Queries.GetActivitiesGroups;

public class GetActivitiesGroupsRealmHandle 
  : IPipelineBehavior<GetActivitiesGroupsQuery, PaginationView<List<ActivityGroupView>>>
{
  private readonly IUserRepository _userRepository;

  public GetActivitiesGroupsRealmHandle (IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }
  
  public async Task<PaginationView<List<ActivityGroupView>>> Handle (GetActivitiesGroupsQuery request, RequestHandlerDelegate<PaginationView<List<ActivityGroupView>>> next, CancellationToken cancellationToken)
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