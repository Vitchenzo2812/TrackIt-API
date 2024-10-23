using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.CategoryCommands.DeleteCategory;

public class DeleteCategoryHandle : IRequestHandler<DeleteCategoryCommand>
{
  private readonly ICategoryRepository _categoryRepository;
  private readonly IUnitOfWork _unitOfWork;

  public DeleteCategoryHandle (
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork
  )
  {
    _categoryRepository = categoryRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (DeleteCategoryCommand request, CancellationToken cancellationToken)
  {
    var category = await _categoryRepository.FindById(request.Aggregate);

    if (category is null)
      throw new NotFoundError("Category not found");
    
    _categoryRepository.Delete(category.SendDeleteCategoryEvent());
    
    await _unitOfWork.SaveChangesAsync();
  }
}