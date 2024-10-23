using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.MonthlyExpensesCommands.UpdateMonthlyExpenses;

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
    var monthlyExpense = await _monthlyExpensesRepository.FindById(request.Aggregate);

    if (monthlyExpense is null)
      throw new NotFoundError("Monthly expenses not found");

    monthlyExpense
      .WithTitle(request.Payload.Title)
      .WithLimit(request.Payload.Limit);
    
    await _unitOfWork.SaveChangesAsync();
  }
}