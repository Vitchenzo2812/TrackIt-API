using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities;
using MediatR;
using TrackIt.Entities.Activities;

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
      ActivityGroup.Create()
        .WithTitle(request.Payload.Title)
        .WithOrder((groups.Count + 1))
        .AssignUser(request.Session!.Id)
    );

    await _unitOfWork.SaveChangesAsync();
  }
}