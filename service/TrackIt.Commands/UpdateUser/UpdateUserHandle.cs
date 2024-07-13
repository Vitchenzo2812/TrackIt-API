using MediatR;
using TrackIt.Infraestructure.Database.Contracts;
using TrackIt.Infraestructure.Repository.Contracts;

namespace TrackIt.Commands.User.UpdateUser;

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
    if (await _userRepository.FindById(request.AggregateId))
      throw new Not
  }
}