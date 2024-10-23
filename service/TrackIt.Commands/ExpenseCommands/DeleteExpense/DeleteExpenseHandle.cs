using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.ExpenseCommands.DeleteExpense;

public class DeleteExpenseHandle : IRequestHandler<DeleteExpenseCommand>
{
  private readonly IExpenseRepository _expenseRepository;
  private readonly IUnitOfWork _unitOfWork;

  public DeleteExpenseHandle (
    IExpenseRepository expenseRepository,
    IUnitOfWork unitOfWork
  )
  {
    _expenseRepository = expenseRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (DeleteExpenseCommand request, CancellationToken cancellationToken)
  {
    var expense = await _expenseRepository.FindById(request.Aggregate);

    if (expense is null)
      throw new NotFoundError("Expense not found");
    
    _expenseRepository.Delete(expense);
    await _unitOfWork.SaveChangesAsync();
  }
}