using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities;
using MediatR;

namespace TrackIt.Commands.MonthlyExpenseCommands.CreateMonthlyExpense;

public class CreateMonthlyExpensesHandle : IRequestHandler<CreateMonthlyExpensesCommand>
{
  private readonly IMonthlyExpensesRepository _monthlyExpensesRepository;

  private readonly IUnitOfWork _unitOfWork;
    
  public CreateMonthlyExpensesHandle (
    IMonthlyExpensesRepository monthlyExpensesRepository,
    IUnitOfWork unitOfWork
  )
  {
    _monthlyExpensesRepository = monthlyExpensesRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (CreateMonthlyExpensesCommand request, CancellationToken cancellationToken)
  {
    _monthlyExpensesRepository.Save(
      MonthlyExpenses.Create(
        title: request.Payload.Title,
        description: request.Payload.Description,
        userId: request.Session!.Id
      )
    );
    
    await _unitOfWork.SaveChangesAsync();
  }
}