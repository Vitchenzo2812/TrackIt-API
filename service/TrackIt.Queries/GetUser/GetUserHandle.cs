using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Errors;
using TrackIt.Queries.Views;
using MediatR;

namespace TrackIt.Queries.GetUser;

public class GetUserHandle : IRequestHandler<GetUserQuery, UserView>
{
  private readonly TrackItDbContext _db;

  public GetUserHandle (TrackItDbContext db)
  {
    _db = db;
  }
  
  public async Task<UserView> Handle (GetUserQuery request, CancellationToken cancellationToken)
  {
    var user = await _db.User
      .Include(u => u.Password)
      .FirstOrDefaultAsync(u => u.Id == request.Params.UserId, cancellationToken);

    if (user is null)
      throw new NotFoundError("User not found");
    
    return UserView.Build(user);
  }
}