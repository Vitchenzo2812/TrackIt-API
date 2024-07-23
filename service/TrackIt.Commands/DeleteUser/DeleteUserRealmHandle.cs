﻿using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Entities.Errors;
using TrackIt.Entities;
using MediatR;

namespace TrackIt.Commands.DeleteUser;

public class DeleteUserRealmHandle : IPipelineBehavior<DeleteUserCommand, Unit>
{
  private readonly IUserRepository _userRepository;

  public DeleteUserRealmHandle (
    IUserRepository userRepository
  )
  {
    _userRepository = userRepository;
  }
  
  public async Task<Unit> Handle (DeleteUserCommand request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken)
  {
    if (request.Session is null)
      throw new ForbiddenError();
    
    if (await _userRepository.FindById(request.Session.Id) is null)
      throw new NotFoundError("Session user not found");
    
    if (await _userRepository.FindById(request.AggregateId) is null)
      throw new NotFoundError("User not found");
    
    if (request.Session.Id == request.AggregateId || request.Session.Hierarchy == Hierarchy.ADMIN)
      return await next();
    
    throw new ForbiddenError();
  }
}