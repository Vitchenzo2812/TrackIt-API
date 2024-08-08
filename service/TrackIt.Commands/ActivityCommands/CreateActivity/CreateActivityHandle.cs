using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities;
using MediatR;

namespace TrackIt.Commands.ActivityCommands.CreateActivity;

public class CreateActivityHandle : IRequestHandler<CreateActivityCommand>
{
  private readonly IActivityRepository _activityRepository;
  
  private readonly IUnitOfWork _unitOfWork;
  
  public CreateActivityHandle (
    IActivityRepository activityRepository,
    IUnitOfWork unitOfWork
  )
  {
    _activityRepository = activityRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (CreateActivityCommand request, CancellationToken cancellationToken)
  {
    var activities = await _activityRepository.GetActivitiesByGroup(request.AggregateId);
    
    _activityRepository.Save(
      Activity.Create(
        activityGroupId: request.AggregateId,
        title: request.Payload.Title,
        order: (activities.Count + 1),
        description: request.Payload.Description
      )
    );
    
    await _unitOfWork.SaveChangesAsync();
  }
}