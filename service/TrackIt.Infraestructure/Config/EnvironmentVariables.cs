using System.ComponentModel;

namespace TrackIt.Infraestructure.Config;

public enum EnvironmentVariables
{
  [Description("MYSQL_TRACKIT_CONNECTION_STRING")]
  MySqlTrackItConnectionString
}