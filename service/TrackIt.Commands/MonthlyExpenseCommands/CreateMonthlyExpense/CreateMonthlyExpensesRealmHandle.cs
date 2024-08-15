using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.MonthlyExpenseCommands.CreateMonthlyExpense;

public class CreateMonthlyExpensesRealmHandle : IPipelineBehavior<CreateMonthlyExpensesCommand, Unit>
{
  private readonly IUserRepository _userRepository;
  
  private readonly IMonthlyExpensesRepository _monthlyExpensesRepository;

  public CreateMonthlyExpensesRealmHandle (
    IUserRepository userRepository,
    IMonthlyExpensesRepository monthlyExpensesRepository
  )
  {
    _userRepository = userRepository;
    _monthlyExpensesRepository = monthlyExpensesRepository;
  }
  
  public async Task<Unit> Handle (CreateMonthlyExpensesCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();

    return await next();
  }
}