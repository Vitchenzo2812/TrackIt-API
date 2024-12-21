using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Errors;
using MediatR;
using MySqlConnector;

namespace TrackIt.Queries.GetCategory;

public class GetCategoryHandle : IRequestHandler<GetCategoryQuery, GetCategoryResult>
{
  private readonly TrackItDbContext _db;

  public GetCategoryHandle (TrackItDbContext db) => _db = db;
  
  public async Task<GetCategoryResult> Handle (GetCategoryQuery request, CancellationToken cancellationToken)
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
          WHERE category.Id = @CategoryId
         """,
        new MySqlParameter("@CategoryId", request.Params.CategoryId)
      ).FirstOrDefaultAsync();

    if (sql is null)
      throw new NotFoundError("Category not found");
    
    return GetCategoryResult.Build(sql);
  }
}