using TrackIt.Entities.Core;

namespace TrackIt.Entities.Repository;

public interface IRepository<TEntity> where TEntity : Entity
{
  Task<TEntity?> FindById (Guid aggregateId);
  
  void Save (TEntity aggregate);

  void Delete (TEntity aggregate);
}