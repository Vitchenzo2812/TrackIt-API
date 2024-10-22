using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities;
using MediatR;
using TrackIt.Entities.Activities;
using TrackIt.Entities.Repository;

namespace TrackIt.Commands.SubActivityCommands.CreateSubActivity;

public class CreateSubActivityHandle : IRequestHandler<CreateSubActivityCommand>
{
  private readonly ISubActivityRepository _subActivityRepository;
  private readonly IUnitOfWork _unitOfWork;

  public CreateSubActivityHandle (
    ISubActivityRepository subActivityRepository,
    IUnitOfWork unitOfWork
  )
  {
    _subActivityRepository = subActivityRepository;
    _unitOfWork = unitOfWork;
  } 
  
  public async Task Handle (CreateSubActivityCommand request, CancellationToken cancellationToken)
  {
    _subActivityRepository.Save(
      SubActivity.Create()
        .AssignToActivity(request.ActivitySubActivityAggregate.ActivityId)
        .WithTitle(request.Payload.Title)
        .WithDescription(request.Payload.Description)
        .WithPriority(request.Payload.Priority)
        .WithOrder(request.Payload.Order)
    );
    
    await _unitOfWork.SaveChangesAsync();
  }
}