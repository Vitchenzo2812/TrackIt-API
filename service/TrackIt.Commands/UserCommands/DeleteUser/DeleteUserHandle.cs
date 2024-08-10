using MediatR;
using TrackIt.Entities.Errors;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Infraestructure.Repository.Contracts;

namespace TrackIt.Commands.UserCommands.DeleteUser;

public class DeleteUserHandle : IRequestHandler<DeleteUserCommand>
{
  private readonly IUserRepository _userRepository;

  private readonly IUnitOfWork _unitOfWork;
  
  public DeleteUserHandle (
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
  )
  {
    _userRepository = userRepository;
    _unitOfWork = unitOfWork;
  }
  
  public async Task Handle (DeleteUserCommand request, CancellationToken cancellationToken)
  {
    var user = await _userRepository.FindById(request.Aggregate);

    if (user is null)
      throw new NotFoundError("User not found");
    
    _userRepository.Delete(user);

    await _unitOfWork.SaveChangesAsync();
  }
}