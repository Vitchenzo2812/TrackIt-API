using TrackIt.Entities.Core;

namespace TrackIt.Infraestructure.Repository.Contracts;

public interface IRepository<TAggregate> where TAggregate : Aggregate
{
  Task<TAggregate?> FindById (Guid aggregateId);
  
  void Save (TAggregate aggregate);

  void Delete (TAggregate aggregate);
  
  void Update (TAggregate aggregate);
}