using TrackIt.Entities.Repository;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.CategoryCommands.CreateCategory;

public class CreateCategoryRealmHandle : IPipelineBehavior<CreateCategoryCommand, Unit>
{
  private readonly IUserRepository _userRepository;

  public CreateCategoryRealmHandle (
    IUserRepository userRepository
  )
  {
    _userRepository = userRepository;
  }
  
  public async Task<Unit> Handle (CreateCategoryCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
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