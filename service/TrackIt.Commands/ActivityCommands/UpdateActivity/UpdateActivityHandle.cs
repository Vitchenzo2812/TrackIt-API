using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Errors;
using MediatR;
using TrackIt.Entities.Repository;

namespace TrackIt.Commands.ActivityCommands.UpdateActivity;

public class UpdateActivityHandle : IRequestHandler<UpdateActivityCommand>
{
  private readonly IActivityRepository _activityRepository;
  private readonly IUnitOfWork _unitOfWork;

  public UpdateActivityHandle (
    IActivityRepository activityRepository,
    IUnitOfWork unitOfWork
  )
  {
    _activityRepository = activityRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (UpdateActivityCommand request, CancellationToken cancellationToken)
  {
    var activity = await _activityRepository.FindById(request.ActivitySubActivityAggregate.ActivityId);

    if (activity is null)
      throw new NotFoundError("Activity not found");

    activity
      .WithTitle(request.Payload.Title)
      .WithDescription(request.Payload.Description)
      .WithPriority(request.Payload.Priority)
      .WithOrder(request.Payload.Order)
      .ShouldCheck(request.Payload.IsChecked);
    
    await _unitOfWork.SaveChangesAsync();
  }
}