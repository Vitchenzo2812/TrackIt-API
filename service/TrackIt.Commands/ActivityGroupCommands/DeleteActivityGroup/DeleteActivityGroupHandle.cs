﻿using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Errors;
using MediatR;
using TrackIt.Entities.Repository;

namespace TrackIt.Commands.ActivityGroupCommands.DeleteActivityGroup;

public class DeleteActivityGroupHandle: IRequestHandler<DeleteActivityGroupCommand>
{
  private readonly IActivityGroupRepository _activityGroupRepository;
  private readonly IUnitOfWork _unitOfWork;

  public DeleteActivityGroupHandle (
    IActivityGroupRepository activityGroupRepository,
    IUnitOfWork unitOfWork
  )
  {
    _activityGroupRepository = activityGroupRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (DeleteActivityGroupCommand request, CancellationToken cancellationToken)
  {
    var group = await _activityGroupRepository.FindById(request.Aggregate);

    if (group is null)
      throw new NotFoundError("Activity Group not found");
    
    _activityGroupRepository.Delete(group);
    await _unitOfWork.SaveChangesAsync();
  }
}