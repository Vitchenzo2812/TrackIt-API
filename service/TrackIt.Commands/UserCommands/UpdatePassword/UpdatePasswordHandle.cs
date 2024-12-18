﻿using MediatR;
using TrackIt.Commands.Errors;
using TrackIt.Entities;
using TrackIt.Entities.Errors;
using TrackIt.Entities.Repository;
using TrackIt.Infraestructure.Database.Contracts;

namespace TrackIt.Commands.UserCommands.UpdatePassword;

public class UpdatePasswordHandle : IRequestHandler<UpdatePasswordCommand>
{
  private readonly IUserRepository _userRepository;
  
  private readonly IUnitOfWork _unitOfWork;

  public UpdatePasswordHandle (
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
  )
  {
    _userRepository = userRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (UpdatePasswordCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.FindById(request.Session!.Id);

    if (user is null)
      throw new NotFoundError("User not found");

    if (Password.Verify(request.Payload.NewPassword, user.Password!))
      throw new PasswordCannotBeTheSameError();

    user.UpdatePassword(request.Payload.NewPassword);
    
    await _unitOfWork.SaveChangesAsync();
  }
}