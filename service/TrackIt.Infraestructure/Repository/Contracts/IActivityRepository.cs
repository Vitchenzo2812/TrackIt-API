using TrackIt.Entities;

namespace TrackIt.Infraestructure.Repository.Contracts;

public interface IActivityRepository : IRepository<Activity>
{
  Task<List<Activity>> GetActivitiesByGroup (Guid activityGroupId);
}