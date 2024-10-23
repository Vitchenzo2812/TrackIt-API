using TrackIt.Entities.Repository;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.CategoryCommands.UpdateCategory;

public class UpdateCategoryRealmHandle : IPipelineBehavior<UpdateCategoryCommand, Unit>
{
  private readonly IUserRepository _userRepository;
  private readonly ICategoryRepository _categoryRepository;
  private readonly ICategoryConfigRepository _categoryConfigRepository;
  
  public UpdateCategoryRealmHandle (
    IUserRepository userRepository,
    ICategoryRepository categoryRepository,
    ICategoryConfigRepository categoryConfigRepository
  )
  {
    _userRepository = userRepository;
    _categoryRepository = categoryRepository;
    _categoryConfigRepository = categoryConfigRepository;
  }
  
  public async Task<Unit> Handle (UpdateCategoryCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();

    var category = await _categoryRepository.FindById(request.Aggregate);

    if (category is null)
      throw new NotFoundError("Category not found");

    if (await _categoryConfigRepository.FindByCategoryId(category.Id) is null)
      throw new NotFoundError("Category config not found");
    
    return await next();
  }
}