using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Errors;
using MediatR;

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
    var activity = await _activityRepository.FindById(request.Payload.ActivityId);

    if (activity is null)
      throw new NotFoundError("Activity not found");

    activity.Update(
      title: request.Payload.Title,
      description: request.Payload.Description,
      order: request.Payload.Order,
      isChecked: request.Payload.Checked
    );
    
    await _unitOfWork.SaveChangesAsync();
  }
}