using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Web.Dto;
using Microsoft.EntityFrameworkCore;
using TrackIt.Commands.Auth.SignUp;
using TrackIt.Tests.Config;
using TrackIt.Entities;
using System.Net;
using Moq;
using TrackIt.Entities.Events;
using TrackIt.Infraestructure.Mailer.Models;

namespace TrackIt.Tests.Integration.Auth;

public class SignUpTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldSignUpAndSendEmail ()
  {
    var payload = new SignUpPayload(
      Email: "gvitchenzo@gmail.com",
      Password: "PasswordTest@1234"
    );

    var response = await _httpClient.PostAsync("/auth/sign-up", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    
    var created = _db.User
      .Include(u => u.Password)
      .AsEnumerable()
      .FirstOrDefault(u => u.Email.Value == payload.Email);
    
    Assert.NotNull(created);
    Assert.NotNull(created.Email);
    Assert.NotNull(created.Password);
    Assert.Equal("gvitchenzo@gmail.com", created.Email.Value);
    Assert.True(Password.Verify("PasswordTest@1234", created.Password));
    
    Assert.NotNull(_harness.Published.Select(p => p.MessageObject.GetType() == typeof(SignUpEvent)).FirstOrDefault());
  }

  [Fact]
  public async Task ShouldReturnEmailAlreadyInUse ()
  {
    var payload1 = new SignUpPayload(
      Email: "gvitchenzo@gmail.com",
      Password: "PasswordTest@1234"
    );

    await _httpClient.PostAsync("/auth/sign-up", payload1.ToJson());
    
    var payload2 = new SignUpPayload(
      Email: "gvitchenzo@gmail.com",
      Password: "DiffPassword_1234"
    );

    var response = await _httpClient.PostAsync("/auth/sign-up", payload2.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Email already in use!", result.Message);
    Assert.Equal("EMAIL_ALREADY_IN_USE", result.Code);
    Assert.Equal(400, result.StatusCode);
  }
  
  [Fact]
  public async Task ShouldReturnInvalidEmail ()
  {
    var payload = new SignUpPayload(
      Email: "some_wrong_email",
      Password: "PasswordTest@1234"
    );

    var response = await _httpClient.PostAsync("/auth/sign-up", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Invalid email format", result.Message);
    Assert.Equal("INVALID_EMAIL_FORMAT", result.Code);
    Assert.Equal(400, result.StatusCode);
  }
  
  [Fact]
  public async Task ShouldReturnInvalidPassword ()
  {
    var payload = new SignUpPayload(
      Email: "gvitchenzo@gmail.com",
      Password: "some_wrong_password"
    );

    var response = await _httpClient.PostAsync("/auth/sign-up", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Password must be contains upper case letter", result.Message);
    Assert.Equal("INVALID_PASSWORD_FORMAT", result.Code);
    Assert.Equal(400, result.StatusCode);
  }
}