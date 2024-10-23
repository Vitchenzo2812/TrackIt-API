using TrackIt.Entities.Repository;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.ExpenseCommands.CreateExpense;

public class CreateExpenseRealmHandle : IPipelineBehavior<CreateExpenseCommand, Unit>
{
  private readonly IUserRepository _userRepository;
  private readonly ICategoryRepository _categoryRepository;
  private readonly IPaymentFormatRepository _paymentFormatRepository;

  public CreateExpenseRealmHandle (
    IUserRepository userRepository,
    ICategoryRepository categoryRepository,
    IPaymentFormatRepository paymentFormatRepository
  )
  {
    _userRepository = userRepository;
    _categoryRepository = categoryRepository;
    _paymentFormatRepository = paymentFormatRepository;
  }
  
  public async Task<Unit> Handle (CreateExpenseCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();

    if (await _categoryRepository.FindById(request.Payload.CategoryId) is null)
      throw new NotFoundError("Category not found");
    
    if (await _paymentFormatRepository.FindById(request.Payload.PaymentFormatId) is null)
      throw new NotFoundError("Payment format not found");
    
    return await next();
  }
}