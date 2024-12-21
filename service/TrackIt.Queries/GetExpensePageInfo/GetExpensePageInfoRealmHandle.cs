using TrackIt.Entities.Repository;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Queries.GetExpensePageInfo;

public class GetExpensePageInfoRealmHandle : IPipelineBehavior<GetExpensePageInfoQuery, List<GetExpensePageInfoResult>>
{
  private readonly IUserRepository _userRepository;

  public GetExpensePageInfoRealmHandle (
    IUserRepository userRepository
  )
  {
    _userRepository = userRepository;
  }
  
  public async Task<List<GetExpensePageInfoResult>> Handle (GetExpensePageInfoQuery request, RequestHandlerDelegate<List<GetExpensePageInfoResult>> next, CancellationToken cancellationToken)
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