using TrackIt.Infraestructure.EventBus.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Events;
using MassTransit;

namespace TrackIt.Events.Consumers;

public class UpdateCategoryConsumer : IEventConsumer<UpdateCategoryEvent>
{
  private readonly ICategoryConfigRepository _categoryConfigRepository;
  private readonly ICategoryRepository _categoryRepository;
  private readonly IUnitOfWork _unitOfWork;

  public UpdateCategoryConsumer (
    ICategoryConfigRepository categoryConfigRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork
  )
  {
    _categoryConfigRepository = categoryConfigRepository;
    _categoryRepository = categoryRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Consume (ConsumeContext<UpdateCategoryEvent> @event)
  {
    var category = await _categoryRepository.FindById(@event.Message.CategoryId);

    if (category is null)
      return;

    var config = await _categoryConfigRepository.FindByCategoryId(category.Id);

    if (config is null)
      return;

    config
      .WithIcon(@event.Message.Icon)
      .WithIconColor(@event.Message.IconColor)
      .WithBackgroundIconColor(@event.Message.BackgroundIconColor);
    
    await _unitOfWork.SaveChangesAsync();
  }
}