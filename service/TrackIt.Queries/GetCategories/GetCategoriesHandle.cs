using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace TrackIt.Queries.GetCategories;

public class GetCategoriesHandle : IRequestHandler<GetCategoriesQuery, List<GetCategoriesResult>>
{
  private readonly TrackItDbContext _db;

  public GetCategoriesHandle (TrackItDbContext db) => _db = db;
    
  public async Task<List<GetCategoriesResult>> Handle (GetCategoriesQuery request, CancellationToken cancellationToken)
  {
    var sql = await _db.Database
      .SqlQueryRaw<CategoryRow>(
        $"""
          SELECT
            category.Id AS Id
           ,category.Title AS Title
           ,category.Description AS Description
           ,config.Icon AS Icon
           ,config.IconColor AS IconColor
           ,config.BackgroundIconColor AS BackgroundIconColor
          FROM Categories category
          RIGHT JOIN CategoryConfigs config ON category.Id = config.CategoryId   
         """
      ).ToListAsync();

    return sql.Select(GetCategoriesResult.Build).ToList();
  }
}