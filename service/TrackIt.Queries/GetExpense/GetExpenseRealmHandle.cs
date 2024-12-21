using TrackIt.Entities.Repository;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Queries.GetExpense;

public class GetExpenseRealmHandle : IPipelineBehavior<GetExpenseQuery, GetExpenseResult>
{
  private readonly IUserRepository _userRepository;
  private readonly IExpenseRepository _expenseRepository;
  
  public GetExpenseRealmHandle (
    IUserRepository userRepository,
    IExpenseRepository expenseRepository
  )
  {
    _userRepository = userRepository;
    _expenseRepository = expenseRepository;
  }
  
  public async Task<GetExpenseResult> Handle (GetExpenseQuery request, RequestHandlerDelegate<GetExpenseResult> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();

    if (await _expenseRepository.FindById(request.Params.ExpenseId) is null)
      throw new NotFoundError("Expense not found");
    
    return await next();
  }
}