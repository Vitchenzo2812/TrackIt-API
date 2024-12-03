using TrackIt.Entities.Activities;

namespace TrackIt.Entities.Repository;

public interface ISubActivityRepository : IRepository<SubActivity>
{
  Task<List<SubActivity>> FindByActivityId (Guid activityId);
}