using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Errors;
using MediatR;
using TrackIt.Entities.Repository;

namespace TrackIt.Commands.ActivityCommands.DeleteActivity;

public class DeleteActivityHandle : IRequestHandler<DeleteActivityCommand>
{
  private readonly IActivityRepository _activityRepository;
  private readonly IUnitOfWork _unitOfWork;

  public DeleteActivityHandle (
    IActivityRepository activityRepository,
    IUnitOfWork unitOfWork
  )
  {
    _activityRepository = activityRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (DeleteActivityCommand request, CancellationToken cancellationToken)
  {
    var activity = await _activityRepository.FindById(request.Aggregate.ActivityId);

    if (activity is null)
      throw new NotFoundError("Activity not found");

    _activityRepository.Delete(activity);
    await _unitOfWork.SaveChangesAsync();
  }
}