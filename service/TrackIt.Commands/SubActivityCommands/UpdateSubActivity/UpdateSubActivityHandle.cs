﻿using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Errors;
using MediatR;
using TrackIt.Entities.Repository;

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
    if (request.Aggregate.SubActivityId is null)
      throw new ForbiddenError("SubActivityId not provided");
    
    var subActivity = await _subActivityRepository.FindById((Guid)request.Aggregate.SubActivityId);
    
    if (subActivity is null)
      throw new NotFoundError("SubActivity not found");

    subActivity
      .WithTitle(request.Payload.Title)
      .WithDescription(request.Payload.Description)
      .WithPriority(request.Payload.Priority)
      .WithOrder(request.Payload.Order)
      .ShouldCheck(request.Payload.IsChecked);
    
    await _unitOfWork.SaveChangesAsync();
  }
}