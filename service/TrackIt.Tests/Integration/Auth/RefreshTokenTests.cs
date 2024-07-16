using TrackIt.Commands.Auth.RefreshToken;
using TrackIt.Commands.Auth.SignIn;
using TrackIt.Entities;
using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Security.Models;
using TrackIt.Infraestructure.Web.Dto;
using PartialSession = TrackIt.Entities.Core.Session;
using TrackIt.Tests.Config;
using TrackIt.Tests.Mocks;

namespace TrackIt.Tests.Integration.Auth;

public class RefreshTokenTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldReturnSession ()
  {
    var user = await CreateUser();

    var partialSession = PartialSession.Create(user);
    var token = _jwtService.GenerateToken(partialSession);
    var refreshToken = _refreshTokenService.GenerateToken();

    await _refreshTokenService.SaveToken(partialSession, refreshToken);

    var payload = new RefreshTokenPayload(
      Token: token,
      RefreshToken: refreshToken
    );
    
    var response = await _httpClient.PostAsync("/auth/refresh-token", payload.ToJson());
    var session = await response.ToData<Session>();
    
    Assert.Equal(session.Id, user.Id);
    Assert.Equal(session.Name, user.Name);
    Assert.Equal(session.Hierarchy, user.Hierarchy);
    Assert.Equal(session.Income, user.Income);
    Assert.Equal(session.Email, user.Email.Value);
    Assert.NotNull(session.Token);
    Assert.NotNull(session.RefreshToken);
  }

  [Fact]
  public async Task ShouldThrowCannotRefreshToken ()
  {
    var user = await CreateUser();

    var partialSession = PartialSession.Create(user);
    var token = _jwtService.GenerateToken(partialSession);
    await _refreshTokenService.SaveToken(partialSession, _refreshTokenService.GenerateToken());

    var payload = new RefreshTokenPayload(
      Token: token,
      RefreshToken: "Diff_refresh_token"
    );
      
    var response = await _httpClient.PostAsync("/auth/refresh-token", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Invalid refresh token request", result.Message);
    Assert.Equal("CANNOT_REFRESH_TOKEN", result.Code);
    Assert.Equal(401, result.StatusCode);
  }

  [Fact]
  public async Task ShouldThrowUserNotFound ()
  {
    var user = UserMock.Build(Password.Create("PasswordTest@1234"));
    
    var partialSession = PartialSession.Create(user);
    var token = _jwtService.GenerateToken(partialSession);
    var refreshToken = _refreshTokenService.GenerateToken();
    
    await _refreshTokenService.SaveToken(partialSession, refreshToken);

    var payload = new RefreshTokenPayload(
      Token: token,
      RefreshToken: refreshToken
    );
      
    var response = await _httpClient.PostAsync("/auth/refresh-token", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("User not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }

  [Fact]
  public async Task ShouldReturnCorrectSessionWithSequencingRefreshTokenRequests ()
  {
    var user = await CreateUser();

    var signInPayload = new SignInPayload (
      Email: user.Email.Value,
      Password: "PasswordTest@1234"
    );

    var signInResponse = await _httpClient.PostAsync("/auth/sign-in", signInPayload.ToJson());
    var signInResult = await signInResponse.ToData<Session>();

    var refreshTokenPayload = new RefreshTokenPayload(
      Token: signInResult.Token,
      RefreshToken: signInResult.RefreshToken
    );
    
    var response1 = await _httpClient.PostAsync("/auth/refresh-token", refreshTokenPayload.ToJson());
    var result1 = await response1.ToData<Session>();

    var payload = new RefreshTokenPayload(
      Token: result1.Token,
      RefreshToken: result1.RefreshToken
    );
      
    var response2 = await _httpClient.PostAsync("/auth/refresh-token", payload.ToJson());
    var result2 = await response2.ToData<Session>();
    
    Assert.Equal(user.Id, result2.Id);
    Assert.Equal(user.Name, result2.Name);
    Assert.Equal(user.Hierarchy, result2.Hierarchy);
    Assert.Equal(user.Income, result2.Income);
    Assert.Equal(user.Email.Value, result2.Email);
    Assert.NotNull(result2.Token);
    Assert.NotNull(result2.RefreshToken);
  }
}