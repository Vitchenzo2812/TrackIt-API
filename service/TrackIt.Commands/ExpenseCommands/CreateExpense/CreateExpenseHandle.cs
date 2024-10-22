using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Services;
using TrackIt.Entities.Expenses;
using MediatR;

namespace TrackIt.Commands.ExpenseCommands.CreateExpense;

public class CreateExpenseHandle : IRequestHandler<CreateExpenseCommand>
{
  private readonly IExpenseRepository _expenseRepository;
  private readonly MonthlyExpensesService _monthlyExpensesService;
  private readonly IUnitOfWork _unitOfWork;

  public CreateExpenseHandle (
    IExpenseRepository expenseRepository,
    MonthlyExpensesService monthlyExpensesService,
    IUnitOfWork unitOfWork
  )
  {
    _expenseRepository = expenseRepository;
    _monthlyExpensesService = monthlyExpensesService;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (CreateExpenseCommand request, CancellationToken cancellationToken)
  {
    var expense = Expense.Create()
      .WithTitle(request.Payload.Title)
      .WithDescription(request.Payload.Description)
      .WithDate(request.Payload.Date)
      .WithAmount(request.Payload.Amount)
      .IsRecurringExpense(request.Payload.IsRecurring)
      .AssignToCategory(request.Payload.CategoryId)
      .AssignToPaymentFormat(request.Payload.PaymentFormatId);

    await _monthlyExpensesService.AddExpenseToMonthlyGroup(request.Session!.Id, expense);
    
    _expenseRepository.Save(expense);
    
    await _unitOfWork.SaveChangesAsync();
  }
}