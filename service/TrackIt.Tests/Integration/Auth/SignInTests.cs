using TrackIt.Infraestructure.Security.Models;
using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Web.Dto;
using TrackIt.Commands.Auth.SignIn;
using TrackIt.Tests.Config;
using System.Net;

namespace TrackIt.Tests.Integration.Auth;

public class SignInTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldSignIn ()
  {
    var user = await CreateUser();
    
    var payload = new SignInPayload(
      Email: "gvitchenzo@gmail.com",
      Password: "PasswordTest@1234"
    );

    var response = await _httpClient.PostAsync("/auth/sign-in", payload.ToJson());
    var result = await response.ToData<Session>();
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    
    Assert.NotNull(result.Token);
    Assert.NotNull(result.RefreshToken);
    Assert.Equal(user.Id, result.Id);
    Assert.Equal(user.Name, result.Name);
    Assert.Equal(user.Income, result.Income);
    Assert.Equal(user.Email?.Value, result.Email);
    Assert.Equal(user.Hierarchy, result.Hierarchy);
  }

  [Fact]
  public async Task ShouldThrowNotFoundUser ()
  {
    var payload = new SignInPayload(
      Email: "gvitchenzo@gmail.com",
      Password: "PasswordTest@1234"
    );

    var response = await _httpClient.PostAsync("/auth/sign-in", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("User not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }

  [Fact]
  public async Task ShouldWrongEmailOrPassword ()
  {
    await CreateUser();
    
    var payload = new SignInPayload(
      Email: "gvitchenzo@gmail.com",
      Password: "DiffPassword@4561"
    );

    var response = await _httpClient.PostAsync("/auth/sign-in", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Wrong email or password!", result.Message);
    Assert.Equal("WRONG_EMAIL_OR_PASSWORD", result.Code);
    Assert.Equal(400, result.StatusCode);
  }
}