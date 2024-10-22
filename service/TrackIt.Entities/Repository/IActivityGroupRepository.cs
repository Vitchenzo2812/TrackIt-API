using TrackIt.Entities.Activities;

namespace TrackIt.Entities.Repository;

public interface IActivityGroupRepository : IRepository<ActivityGroup>
{
  Task<List<ActivityGroup>> GetAll ();
}