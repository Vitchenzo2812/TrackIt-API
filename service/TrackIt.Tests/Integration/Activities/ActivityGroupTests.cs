using TrackIt.Commands.ActivityGroupCommands.CreateActivityGroup;
using TrackIt.Infraestructure.Extensions;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Config.Builders;
using TrackIt.Tests.Config;
using System.Net;

namespace TrackIt.Tests.Integration.Activities;

public class ActivityGroupTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldCreateActivityGroup ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    const string titleActivityGroup = "GROUP_1";
    
    var payload = new CreateActivityGroupPayload(titleActivityGroup);
    var response = await _httpClient.PostAsync("/activity/group", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    var created = await _db.ActivityGroups.ToListAsync();

    Assert.Single(created);
    Assert.Equal(titleActivityGroup, created[0].Title);
    Assert.Equal(1, created[0].Order);
  }
}