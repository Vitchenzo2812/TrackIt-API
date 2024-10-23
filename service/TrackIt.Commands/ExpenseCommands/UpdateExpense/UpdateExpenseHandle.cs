using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Services;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.ExpenseCommands.UpdateExpense;

public class UpdateExpenseHandle : IRequestHandler<UpdateExpenseCommand>
{
  private readonly MonthlyExpensesService _monthlyExpensesService;
  private readonly IExpenseRepository _expenseRepository;
  private readonly IUnitOfWork _unitOfWork;

  public UpdateExpenseHandle (
    MonthlyExpensesService monthlyExpensesService,
    IExpenseRepository expenseRepository,
    IUnitOfWork unitOfWork
  )
  {
    _monthlyExpensesService = monthlyExpensesService;
    _expenseRepository = expenseRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (UpdateExpenseCommand request, CancellationToken cancellationToken)
  {
    var expense = await _expenseRepository.FindById(request.Aggregate);

    if (expense is null)
      throw new NotFoundError("Expense not found");

    expense
      .WithTitle(request.Payload.Title)
      .WithDescription(request.Payload.Description)
      .WithDate(request.Payload.Date)
      .WithAmount(request.Payload.Amount)
      .IsRecurringExpense(request.Payload.IsRecurring)
      .AssignToPaymentFormat(request.Payload.PaymentFormatId)
      .AssignToCategory(request.Payload.CategoryId);

    await _monthlyExpensesService.AddExpenseToMonthlyGroup(request.Session!.Id, expense);
    
    await _unitOfWork.SaveChangesAsync();
  }
}