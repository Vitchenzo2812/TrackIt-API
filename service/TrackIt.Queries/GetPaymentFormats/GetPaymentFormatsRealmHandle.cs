using MediatR;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using TrackIt.Entities.Repository;

namespace TrackIt.Queries.GetPaymentFormats;

public class GetPaymentFormatsRealmHandle : IPipelineBehavior<GetPaymentFormatsQuery, List<GetPaymentFormatsResult>>
{
  private readonly IUserRepository _userRepository;

  public GetPaymentFormatsRealmHandle (
    IUserRepository userRepository
  )
  {
    _userRepository = userRepository;
  }
  
  public async Task<List<GetPaymentFormatsResult>> Handle (GetPaymentFormatsQuery request, RequestHandlerDelegate<List<GetPaymentFormatsResult>> next, CancellationToken cancellationToken)
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