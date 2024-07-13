using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Queries.Views;
using MediatR;
using TrackIt.Entities.Errors;

namespace TrackIt.Queries.GetUser;

public class GetUserRealmHandle : IPipelineBehavior<GetUserQuery, UserView>
{
  private readonly IUserRepository _userRepository;

  public GetUserRealmHandle (IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }
  
  public async Task<UserView> Handle (GetUserQuery request, RequestHandlerDelegate<UserView> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Params.UserId);

    if (user is null)
      throw new NotFoundError("User not found");

    if (request.Session.Id != user.Id)
      throw new ForbiddenError();

    return await next();
  }
}