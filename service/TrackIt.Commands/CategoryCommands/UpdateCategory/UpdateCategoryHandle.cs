using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.CategoryCommands.UpdateCategory;

public class UpdateCategoryHandle : IRequestHandler<UpdateCategoryCommand>
{
  private readonly ICategoryRepository _categoryRepository;
  private readonly IUnitOfWork _unitOfWork;

  public UpdateCategoryHandle (
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork
  )
  {
    _categoryRepository = categoryRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (UpdateCategoryCommand request, CancellationToken cancellationToken)
  {
    var category = await _categoryRepository.FindById(request.Aggregate);

    if (category is null)
      throw new NotFoundError("Category not found");

    category
      .WithTitle(request.Payload.Title)
      .WithDescription(request.Payload.Description)
      .SendUpdateCategoryEvent(
        request.Payload.Icon,
        request.Payload.IconColor,
        request.Payload.BackgroundIconColor
      );
    
    await _unitOfWork.SaveChangesAsync();
  }
}