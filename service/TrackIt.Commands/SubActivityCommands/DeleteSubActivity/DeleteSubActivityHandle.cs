using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Errors;
using MediatR;
using TrackIt.Entities.Repository;

namespace TrackIt.Commands.SubActivityCommands.DeleteSubActivity;

public class DeleteSubActivityHandle : IRequestHandler<DeleteSubActivityCommand>
{
  private readonly ISubActivityRepository _subActivityRepository;
  private readonly IUnitOfWork _unitOfWork;

  public DeleteSubActivityHandle (
    ISubActivityRepository subActivityRepository,
    IUnitOfWork unitOfWork
  )
  {
    _subActivityRepository = subActivityRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (DeleteSubActivityCommand request, CancellationToken cancellationToken)
  {
    if (request.ActivitySubActivityAggregate.SubActivityId is null)
      throw new ForbiddenError("SubActivityId not provided");
    
    var subActivity = await _subActivityRepository.FindById((Guid)request.ActivitySubActivityAggregate.SubActivityId);
    
    if (subActivity is null)
      throw new NotFoundError("SubActivity not found");

    _subActivityRepository.Delete(subActivity);
    await _unitOfWork.SaveChangesAsync();
  }
}