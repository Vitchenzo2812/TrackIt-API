using TrackIt.Entities.Repository;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.ExpenseCommands.DeleteExpense;

public class DeleteExpenseRealmHandle : IPipelineBehavior<DeleteExpenseCommand, Unit>
{
  private readonly IUserRepository _userRepository;
  private readonly IExpenseRepository _expenseRepository;
  private readonly IMonthlyExpensesRepository _monthlyExpensesRepository;

  public DeleteExpenseRealmHandle (
    IUserRepository userRepository,
    IExpenseRepository expenseRepository,
    IMonthlyExpensesRepository monthlyExpensesRepository
  )
  {
    _userRepository = userRepository;
    _expenseRepository = expenseRepository;
    _monthlyExpensesRepository = monthlyExpensesRepository;
  }
  
  public async Task<Unit> Handle (DeleteExpenseCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();

    var expense = await _expenseRepository.FindById(request.Aggregate);

    if (expense is null)
      throw new NotFoundError("Expense not found");

    if (await _monthlyExpensesRepository.FindById(expense.MonthlyExpensesId) is null)
      throw new NotFoundError("Monthly Expense not found");
    
    return await next();
  }
}