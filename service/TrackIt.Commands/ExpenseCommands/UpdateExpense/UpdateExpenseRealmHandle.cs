using TrackIt.Entities.Repository;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.ExpenseCommands.UpdateExpense;

public class UpdateExpenseRealmHandle : IPipelineBehavior<UpdateExpenseCommand, Unit>
{
  private readonly IUserRepository _userRepository;
  private readonly IExpenseRepository _expenseRepository;
  private readonly ICategoryRepository _categoryRepository;
  private readonly IPaymentFormatRepository _paymentFormatRepository;
  private readonly IMonthlyExpensesRepository _monthlyExpensesRepository;

  public UpdateExpenseRealmHandle (
    IUserRepository userRepository,
    IExpenseRepository expenseRepository,
    ICategoryRepository categoryRepository,
    IPaymentFormatRepository paymentFormatRepository,
    IMonthlyExpensesRepository monthlyExpensesRepository
  )
  {
    _userRepository = userRepository;
    _expenseRepository = expenseRepository;
    _categoryRepository = categoryRepository;
    _paymentFormatRepository = paymentFormatRepository;
    _monthlyExpensesRepository = monthlyExpensesRepository;
  }
  
  public async Task<Unit> Handle (UpdateExpenseCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
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
    
    if (await _categoryRepository.FindById(request.Payload.CategoryId) is null)
      throw new NotFoundError("Category not found");
    
    if (await _paymentFormatRepository.FindById(request.Payload.PaymentFormatId) is null)
      throw new NotFoundError("Payment format not found");
    
    return await next();
  }
}