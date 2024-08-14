using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Errors;
using MediatR;

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
    var subActivity = await _subActivityRepository.FindById(request.Aggregate.SubActivityId);

    if (subActivity is null)
      throw new NotFoundError("SubActivity not found");
    
    _subActivityRepository.Delete(subActivity);
    
    await _unitOfWork.SaveChangesAsync();
  }
}