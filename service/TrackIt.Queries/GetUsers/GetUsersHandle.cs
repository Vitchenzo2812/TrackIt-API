using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Queries.Views;
using MediatR;
using TrackIt.Entities;

namespace TrackIt.Queries.GetUsers;

public class GetUsersHandle : IRequestHandler<GetUsersQuery, PaginationView<List<UserResourceView>>>
{
  private readonly TrackItDbContext _db;

  public GetUsersHandle (TrackItDbContext db)
  {
    _db = db;
  }
  
  public async Task<PaginationView<List<UserResourceView>>> Handle (GetUsersQuery request, CancellationToken cancellationToken)
  {
    var usersQuery = await _db.User.Where(u => u.Hierarchy != Hierarchy.ADMIN).ToListAsync(cancellationToken);

    if (request.Params.Sort is not null && request.Params.Sort.Description() == "RECENTLY")
    {
      usersQuery = usersQuery.OrderByDescending(u => u.CreatedAt).ToList();
    }
    
    if (request.Params.Sort is not null && request.Params.Sort.Description() == "OLD")
    {
      usersQuery = usersQuery.OrderBy(u => u.CreatedAt).ToList();
    }
    
    var users = usersQuery
      .Skip((request.Params.Page - 1) * request.Params.PerPage)
      .Take(request.Params.PerPage)
      .Select(UserResourceView.Build)
      .ToList();
    
    var totalPages = (int)Math.Ceiling((double)usersQuery.Count() / request.Params.PerPage);
    
    return PaginationView<List<UserResourceView>>.Build(request.Params.Page, totalPages, users);
  }
}