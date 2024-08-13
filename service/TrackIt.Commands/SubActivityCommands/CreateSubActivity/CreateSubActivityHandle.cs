using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Errors;
using TrackIt.Entities;
using MediatR;

namespace TrackIt.Commands.SubActivityCommands.CreateSubActivity;

public class CreateSubActivityHandle : IRequestHandler<CreateSubActivityCommand>
{
  private readonly ISubActivityRepository _subActivityRepository;
  
  private readonly IActivityRepository _activityRepository;

  private readonly IUnitOfWork _unitOfWork;
  
  public CreateSubActivityHandle (
    ISubActivityRepository subActivityRepository,
    IActivityRepository activityRepository,
    IUnitOfWork unitOfWork
  )
  {
    _subActivityRepository = subActivityRepository;
    _activityRepository = activityRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (CreateSubActivityCommand request, CancellationToken cancellationToken)
  {
    var activity = await _activityRepository.FindById(request.Aggregate.ActivityId);

    if (activity is null)
      throw new NotFoundError("Activity not found");
    
    _subActivityRepository.Save(
      SubActivity.Create(
        title: request.Payload.Title,
        description: request.Payload.Description,
        isChecked: request.Payload.Checked,
        order: (activity.SubActivities.Count + 1),
        activityId: activity.Id
      )
    );
    
    await _unitOfWork.SaveChangesAsync();
  }
}