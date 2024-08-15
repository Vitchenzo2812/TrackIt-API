using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using TrackIt.Queries.Views;
using MediatR;

namespace TrackIt.Queries.GetMonthlyExpenses;

public class GetMonthlyExpensesRealmHandle : IPipelineBehavior<GetMonthlyExpensesQuery, List<MonthlyExpensesView>>
{
  private readonly IUserRepository _userRepository;
  
  public GetMonthlyExpensesRealmHandle (IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }
  
  public async Task<List<MonthlyExpensesView>> Handle (GetMonthlyExpensesQuery request, RequestHandlerDelegate<List<MonthlyExpensesView>> next, CancellationToken cancellationToken)
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