using TrackIt.Infraestructure.EventBus.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Events;
using MassTransit;

namespace TrackIt.Events.Consumers;

public class DeleteCategoryConsumer : IEventConsumer<DeleteCategoryEvent>
{
  private readonly ICategoryConfigRepository _categoryConfigRepository;
  private readonly IUnitOfWork _unitOfWork;

  public DeleteCategoryConsumer (
    ICategoryConfigRepository categoryConfigRepository,
    IUnitOfWork unitOfWork
  )
  {
    _categoryConfigRepository = categoryConfigRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Consume (ConsumeContext<DeleteCategoryEvent> @event)
  {
    var config = await _categoryConfigRepository.FindByCategoryId(@event.Message.CategoryId);

    if (config is null)
      return;
    
    _categoryConfigRepository.Delete(config);
    await _unitOfWork.SaveChangesAsync();
  }
}