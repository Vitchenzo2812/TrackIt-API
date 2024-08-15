using MediatR;
using TrackIt.Entities;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Infraestructure.Repository.Contracts;

namespace TrackIt.Commands.MonthlyExpenseCommands.CreateMonthlyExpenses;

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