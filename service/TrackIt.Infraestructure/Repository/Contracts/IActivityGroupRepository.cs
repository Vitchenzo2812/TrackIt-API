using TrackIt.Entities;

namespace TrackIt.Infraestructure.Repository.Contracts;

public interface IActivityGroupRepository : IRepository<ActivityGroup>
{
  Task<List<ActivityGroup>> GetAll ();
}