using MediatR;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using TrackIt.Infraestructure.Repository.Contracts;

namespace TrackIt.Commands.MonthlyExpenseCommands.CreateMonthlyExpenses;

public class CreateMonthlyExpensesRealmHandle : IPipelineBehavior<CreateMonthlyExpensesCommand, Unit>
{
  private readonly IUserRepository _userRepository;
  
  public CreateMonthlyExpensesRealmHandle (IUserRepository userRepository)
  {
    _userRepository = userRepository;
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