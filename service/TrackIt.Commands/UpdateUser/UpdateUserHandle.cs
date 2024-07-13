﻿using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Commands.UpdateUser;

public class UpdateUserHandle : IRequestHandler<UpdateUserCommand>
{
  private readonly IUserRepository _userRepository;

  private readonly IUnitOfWork _unitOfWork;

  public UpdateUserHandle (
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
  )
  {
    _userRepository = userRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (UpdateUserCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.FindById(request.AggregateId);
    
    if (user is null)
      throw new NotFoundError("User not found");

    user.Update(
      name: request.Payload.Name,
      income: request.Payload.Income
    );
    
    await _unitOfWork.SaveChangesAsync();
  }
}