using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.SubActivityCommands.UpdateSubActivity;

public class UpdateSubActivityHandle : IRequestHandler<UpdateSubActivityCommand>
{
  private readonly ISubActivityRepository _subActivityRepository;

  private readonly IUnitOfWork _unitOfWork;

  public UpdateSubActivityHandle (
    ISubActivityRepository subActivityRepository,
    IUnitOfWork unitOfWork
  )
  {
    _subActivityRepository = subActivityRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (UpdateSubActivityCommand request, CancellationToken cancellationToken)
  {
    var subActivity = await _subActivityRepository.FindById(request.Aggregate.SubActivityId);

    if (subActivity is null)
      throw new NotFoundError("Sub Activity not found");

    subActivity.Update(
      title: request.Payload.Title,
      description: request.Payload.Description,
      isChecked: request.Payload.Checked,
      order: request.Payload.Order
    );
    
    await _unitOfWork.SaveChangesAsync();
  }
}