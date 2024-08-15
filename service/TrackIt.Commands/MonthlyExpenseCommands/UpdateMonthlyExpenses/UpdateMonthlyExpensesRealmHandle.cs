using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.MonthlyExpenseCommands.UpdateMonthlyExpenses;

public class UpdateMonthlyExpensesRealmHandle : IPipelineBehavior<UpdateMonthlyExpensesCommand, Unit>
{
  private readonly IUserRepository _userRepository;

  private readonly IMonthlyExpensesRepository _monthlyExpensesRepository;
  
  public UpdateMonthlyExpensesRealmHandle (
    IUserRepository userRepository,
    IMonthlyExpensesRepository monthlyExpensesRepository
  )
  {
    _userRepository = userRepository;
    _monthlyExpensesRepository = monthlyExpensesRepository;
  }
  
  public async Task<Unit> Handle (UpdateMonthlyExpensesCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();

    var monthlyExpenses = await _monthlyExpensesRepository.FindById(request.Aggregate);

    if (monthlyExpenses is null)
      throw new NotFoundError("Expense diary not found");
    
    if (monthlyExpenses.UserId == request.Session.Id)
      return await next();

    throw new ForbiddenError();
  }
}