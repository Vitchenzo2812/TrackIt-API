using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Entities.Errors;
using TrackIt.Entities;
using MediatR;

namespace TrackIt.Commands.UpdateUser;

public class UpdateUserRealmHandle : IPipelineBehavior<UpdateUserCommand, Unit>
{
  private readonly IUserRepository _userRepository;

  public UpdateUserRealmHandle (IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }
  
  public async Task<Unit> Handle (UpdateUserCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    if (await _userRepository.FindById(request.Session.Id) is null)
      throw new NotFoundError("Session user not found");
    
    if (await _userRepository.FindById(request.AggregateId) is null)
      throw new NotFoundError("User not found");

    if (request.Session.Id == request.AggregateId || request.Session.Hierarchy == Hierarchy.ADMIN)
      return await next();
    
    throw new ForbiddenError();
  }
}