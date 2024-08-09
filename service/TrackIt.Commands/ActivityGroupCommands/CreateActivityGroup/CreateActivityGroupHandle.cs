using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities;
using MediatR;

namespace TrackIt.Commands.ActivityGroupCommands.CreateActivityGroup;

public class CreateActivityGroupHandle : IRequestHandler<CreateActivityGroupCommand>
{
  private readonly IActivityGroupRepository _activityGroupRepository;

  private readonly IUnitOfWork _unitOfWork;

  public CreateActivityGroupHandle (
    IActivityGroupRepository activityGroupRepository,
    IUnitOfWork unitOfWork
  )
  {
    _activityGroupRepository = activityGroupRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (CreateActivityGroupCommand request, CancellationToken cancellationToken)
  {
    var groups = await _activityGroupRepository.GetAll();
    
    _activityGroupRepository.Save(
      ActivityGroup.Create(
        userId: request.Session!.Id,
        title: request.Payload.Title,
        icon: request.Payload.Icon,
        order: (groups.Count + 1)
      )
    );

    await _unitOfWork.SaveChangesAsync();
  }
}