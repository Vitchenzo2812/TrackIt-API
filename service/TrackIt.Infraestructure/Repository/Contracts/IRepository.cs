using TrackIt.Entities.Core;

namespace TrackIt.Infraestructure.Repository.Contracts;

public interface IRepository<TEntity> where TEntity : Entity
{
  Task<TEntity?> FindById (Guid aggregateId);
  
  void Save (TEntity aggregate);

  void Delete (TEntity aggregate);
  
  void Update (TEntity aggregate);
}