using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities;
using MediatR;

namespace TrackIt.Commands.ExpenseCommands.CreateExpense;

public class CreateExpenseHandle : IRequestHandler<CreateExpenseCommand>
{
  private readonly IExpenseRepository _expenseRepository;

  private readonly IUnitOfWork _unitOfWork;
  
  public CreateExpenseHandle (
    IExpenseRepository expenseRepository,
    IUnitOfWork unitOfWork
  )
  {
    _expenseRepository = expenseRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (CreateExpenseCommand request, CancellationToken cancellationToken)
  {
    _expenseRepository.Save(
      Expense.Create(
        monthlyExpensesId: request.Aggregate,
        date: request.Payload.Date,
        paymentFormat: request.Payload.PaymentFormat,
        amount: request.Payload.Amount,
        description: request.Payload.Description
      )
    );
    
    await _unitOfWork.SaveChangesAsync();
  }
}