using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Expenses;
using MediatR;

namespace TrackIt.Commands.CategoryCommands.CreateCategory;

public class CreateCategoryHandle : IRequestHandler<CreateCategoryCommand>
{
  private readonly ICategoryRepository _categoryRepository;
  private readonly IUnitOfWork _unitOfWork;

  public CreateCategoryHandle (
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork
  )
  {
    _categoryRepository = categoryRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (CreateCategoryCommand request, CancellationToken cancellationToken)
  {
    var category = Category.Create()
      .WithTitle(request.Payload.Title)
      .WithDescription(request.Payload.Description)
      .SendCreateCategoryEvent(
        request.Payload.Icon,
        request.Payload.IconColor,
        request.Payload.BackgroundIconColor
      );
    
    _categoryRepository.Save(category);
    await _unitOfWork.SaveChangesAsync();
  }
}