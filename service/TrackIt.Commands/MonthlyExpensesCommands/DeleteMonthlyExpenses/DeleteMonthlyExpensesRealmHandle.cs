using TrackIt.Entities.Repository;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.MonthlyExpensesCommands.DeleteMonthlyExpenses;

public class DeleteMonthlyExpensesRealmHandle : IPipelineBehavior<DeleteMonthlyExpensesCommand, Unit>
{
  private readonly IUserRepository _userRepository;
  private readonly IMonthlyExpensesRepository _monthlyExpensesRepository;

  public DeleteMonthlyExpensesRealmHandle (
    IUserRepository userRepository,
    IMonthlyExpensesRepository monthlyExpensesRepository
  )
  {
    _userRepository = userRepository;
    _monthlyExpensesRepository = monthlyExpensesRepository;
  }
  
  public async Task<Unit> Handle (DeleteMonthlyExpensesCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();

    if (await _monthlyExpensesRepository.FindById(request.Aggregate) is null)
      throw new NotFoundError("Monthly expenses not found");
    
    return await next();
  }
}