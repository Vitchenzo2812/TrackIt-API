using System.Net;
using TrackIt.Infraestructure.Extensions;
using Microsoft.EntityFrameworkCore;
using TrackIt.Commands.Auth.SignUp;
using TrackIt.Tests.Config;
using TrackIt.Entities;

namespace TrackIt.Tests.Integration.Auth;

public class SignUpTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldSignUp ()
  {
    var payload = new SignUpPayload(
      Email: "gvitchenzo@gmail.com",
      Password: "PasswordTest@1234"
    );

    var response = await _httpClient.PostAsync("/auth/sign-up", payload.ToJson());
    var result = await response.ToData<SignUpResponse>();

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      
    var created = await _db.User
      .Include(u => u.Password)
      .FirstOrDefaultAsync(u => u.Id == result.UserId);
    
    Assert.NotNull(created);
    Assert.NotNull(created.Email);
    Assert.NotNull(created.Password);
    Assert.Equal("gvitchenzo@gmail.com", created.Email.Value);
    Assert.True(Password.Verify("PasswordTest@1234", created.Password));
  }
}