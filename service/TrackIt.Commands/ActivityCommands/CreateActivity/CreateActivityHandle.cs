using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities;
using MediatR;
using TrackIt.Entities.Activities;
using TrackIt.Entities.Repository;

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
    _activityRepository.Save(
      Activity.Create()
        .AssignToGroup(request.ActivitySubActivityAggregate)
        .WithTitle(request.Payload.Title)
        .WithDescription(request.Payload.Description)
        .WithPriority(request.Payload.Priority)
        .WithOrder(request.Payload.Order)
    );
    
    await _unitOfWork.SaveChangesAsync();
  }
}