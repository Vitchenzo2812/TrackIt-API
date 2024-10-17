using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.ActivityGroupCommands.UpdateActivityGroup;

public class UpdateActivityGroupHandle : IRequestHandler<UpdateActivityGroupCommand>
{
  private readonly IActivityGroupRepository _activityGroupRepository;
  private readonly IUnitOfWork _unitOfWork;

  public UpdateActivityGroupHandle (
    IActivityGroupRepository activityGroupRepository,
    IUnitOfWork unitOfWork
  )
  {
    _activityGroupRepository = activityGroupRepository;
    _unitOfWork = unitOfWork;
  } 
  
  public async Task Handle (UpdateActivityGroupCommand request, CancellationToken cancellationToken)
  {
    var group = await _activityGroupRepository.FindById(request.Aggregate);

    if (group is null)
      throw new NotFoundError("Activity Group not found");

    group
      .WithTitle(request.Payload.Title)
      .WithOrder(request.Payload.Order);

    await _unitOfWork.SaveChangesAsync();
  }
}