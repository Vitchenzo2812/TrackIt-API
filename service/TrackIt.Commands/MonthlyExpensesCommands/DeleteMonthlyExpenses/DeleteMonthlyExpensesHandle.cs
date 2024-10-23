using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.MonthlyExpensesCommands.DeleteMonthlyExpenses;

public class DeleteMonthlyExpensesHandle : IRequestHandler<DeleteMonthlyExpensesCommand>
{
  private readonly IMonthlyExpensesRepository _monthlyExpensesRepository;
  private readonly IUnitOfWork _unitOfWork;

  public DeleteMonthlyExpensesHandle (
    IMonthlyExpensesRepository monthlyExpensesRepository,
    IUnitOfWork unitOfWork
  )
  {
    _monthlyExpensesRepository = monthlyExpensesRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (DeleteMonthlyExpensesCommand request, CancellationToken cancellationToken)
  {
    var monthlyExpenses = await _monthlyExpensesRepository.FindById(request.Aggregate);

    if (monthlyExpenses is null)
      throw new NotFoundError("Monthly expenses not found");
    
    _monthlyExpensesRepository.Delete(monthlyExpenses);
    await _unitOfWork.SaveChangesAsync();
  }
}