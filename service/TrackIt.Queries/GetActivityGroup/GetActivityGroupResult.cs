using TrackIt.Entities.Activities;

namespace TrackIt.Queries.GetActivityGroup;

public record GetActivityGroupResult (
  Guid Id,
  string Title,
  int Order
)
{
  public static GetActivityGroupResult Build (ActivityGroup group)
  {
    return new GetActivityGroupResult(
      Id: group.Id,
      Title: group.Title,
      Order: group.Order
    );
  }
}