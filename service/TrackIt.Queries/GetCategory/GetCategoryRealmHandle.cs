using TrackIt.Entities.Repository;
using TrackIt.Commands.Errors;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Queries.GetCategory;

public class GetCategoryRealmHandle : IPipelineBehavior<GetCategoryQuery, GetCategoryResult>
{
  private readonly IUserRepository _userRepository;
  private readonly ICategoryRepository _categoryRepository;
  
  public GetCategoryRealmHandle (
    IUserRepository userRepository,
    ICategoryRepository categoryRepository
  )
  {
    _userRepository = userRepository;
    _categoryRepository = categoryRepository;
  }
  
  public async Task<GetCategoryResult> Handle (GetCategoryQuery request, RequestHandlerDelegate<GetCategoryResult> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();

    var user = await _userRepository.FindById(request.Session.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (!user.EmailValidated)
      throw new EmailMustBeValidatedError();

    if (await _categoryRepository.FindById(request.Params.CategoryId) is null)
      throw new NotFoundError("Category not found");
    
    return await next();
  }
}