namespace TrackIt.Queries.GetActivityGroups;

public record GetActivityGroupsResult (
  Guid Id,
  string Title,
  int Order
)
{
  public static GetActivityGroupsResult Build (Guid id, string title, int order)
  {
    return new GetActivityGroupsResult(
      Id: id,
      Title: title,
      Order: order
    );
  }
}