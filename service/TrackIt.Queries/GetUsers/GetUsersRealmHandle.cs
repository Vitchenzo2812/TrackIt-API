using TrackIt.Entities.Errors;
using TrackIt.Queries.Views;
using TrackIt.Entities;
using MediatR;

namespace TrackIt.Queries.GetUsers;

public class GetUsersRealmHandle : IPipelineBehavior<GetUsersQuery, PaginationView<List<UserResourceView>>>
{
  public async Task<PaginationView<List<UserResourceView>>> Handle (GetUsersQuery request, RequestHandlerDelegate<PaginationView<List<UserResourceView>>> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    if (request.Session.Hierarchy == Hierarchy.ADMIN)
      return await next();
    
    throw new ForbiddenError();
  }
}