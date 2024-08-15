using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.MonthlyExpenseCommands.UpdateMonthlyExpenses;

public class UpdateMonthlyExpensesHandle : IRequestHandler<UpdateMonthlyExpensesCommand>
{
  private readonly IMonthlyExpensesRepository _monthlyExpensesRepository;

  private readonly IUnitOfWork _unitOfWork;
  
  public UpdateMonthlyExpensesHandle (
    IMonthlyExpensesRepository monthlyExpensesRepository,
    IUnitOfWork unitOfWork
  )
  {
    _monthlyExpensesRepository = monthlyExpensesRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (UpdateMonthlyExpensesCommand request, CancellationToken cancellationToken)
  {
    var monthlyExpenses = await _monthlyExpensesRepository.FindById(request.Aggregate);

    if (monthlyExpenses is null)
      throw new NotFoundError("Expense diary not found");

    monthlyExpenses.Update(
      title: request.Payload.Title,
      description: request.Payload.Description
    );
      
    await _unitOfWork.SaveChangesAsync();
  }
}