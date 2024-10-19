using TrackIt.Entities;
using TrackIt.Entities.Activities;

namespace TrackIt.Infraestructure.Repository.Contracts;

public interface IActivityGroupRepository : IRepository<ActivityGroup>
{
  Task<List<ActivityGroup>> GetAll ();
}