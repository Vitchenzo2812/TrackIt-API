using TrackIt.Entities.Repository;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Queries.GetCategories;

public class GetCategoriesRealmHandle : IPipelineBehavior<GetCategoriesQuery, List<GetCategoriesResult>>
{
  private readonly IUserRepository _userRepository;

  public GetCategoriesRealmHandle (
    IUserRepository userRepository
  )
  {
    _userRepository = userRepository;
  }
  
  public async Task<List<GetCategoriesResult>> Handle (GetCategoriesQuery request, RequestHandlerDelegate<List<GetCategoriesResult>> next, CancellationToken cancellationToken)
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