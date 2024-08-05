using PartialSession = TrackIt.Entities.Core.Session;
using TrackIt.Infraestructure.Extensions;
using TrackIt.Commands.UpdatePassword;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Config;
using TrackIt.Entities;
using System.Net;

namespace TrackIt.Tests.Integration.User;

public class UpdatePasswordTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldUpdatePassword ()
  {
    var user = await CreateUser();
    AddAuthorizationData(PartialSession.Create(user));

    var payload = new UpdatePasswordPayload(
      NewPassword: "SomePassword@4321"
    );

    var response = await _httpClient.PutAsync("/user/password", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var updated = await _db.User
      .Include(u => u.Password)
      .FirstOrDefaultAsync(u => u.Id == user.Id);
    
    Assert.NotNull(updated);
    Assert.True(Password.Verify("SomePassword@4321", updated.Password!));
  }
}