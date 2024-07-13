using System.ComponentModel;

namespace TrackIt.Entities;

public enum Hierarchy
{
  [Description("ADMIN")]
  ADMIN = 0,
  
  [Description("CLIENT")]
  CLIENT = 10
}