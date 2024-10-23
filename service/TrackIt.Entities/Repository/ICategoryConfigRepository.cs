using TrackIt.Entities.Expenses;

namespace TrackIt.Entities.Repository;

public interface ICategoryConfigRepository : IRepository<CategoryConfig>
{
  Task<CategoryConfig?> FindByCategoryId (Guid categoryId);
}