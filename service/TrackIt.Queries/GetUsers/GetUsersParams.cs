using System.ComponentModel;

namespace TrackIt.Queries.GetUsers;

public enum Sort
{
  [Description("RECENTLY")]
  RECENTLY,
  
  [Description("OLD")]
  OLD
}

public record GetUsersParams (
  int Page,
  
  int PerPage,
  
  Sort? Sort
);