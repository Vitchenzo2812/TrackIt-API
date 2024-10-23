using TrackIt.Infraestructure.EventBus.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Expenses;
using TrackIt.Entities.Events;
using MassTransit;

namespace TrackIt.Events.Consumers;

public class CreateCategoryConsumer : IEventConsumer<CreateCategoryEvent>
{
  private readonly ICategoryConfigRepository _categoryConfigRepository;
  private readonly ICategoryRepository _categoryRepository; 
  private readonly IUnitOfWork _unitOfWork;

  public CreateCategoryConsumer (
    ICategoryConfigRepository categoryConfigRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork
  )
  {
    _categoryConfigRepository = categoryConfigRepository;
    _categoryRepository = categoryRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Consume (ConsumeContext<CreateCategoryEvent> @event)
  {
    var category = await _categoryRepository.FindById(@event.Message.CategoryId);

    if (category is null)
      return;

    var config = CategoryConfig.Create()
      .WithIcon(@event.Message.Icon)
      .WithIconColor(@event.Message.IconColor)
      .WithBackgroundIconColor(@event.Message.BackgroundIconColor)
      .AssignToCategory(category.Id);
    
    _categoryConfigRepository.Save(config);
    await _unitOfWork.SaveChangesAsync();
  }
}