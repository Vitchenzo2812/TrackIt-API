using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Web.Dto;
using Microsoft.EntityFrameworkCore;
using TrackIt.Commands.Auth.SignUp;
using TrackIt.Entities.Services;
using TrackIt.Entities.Events;
using TrackIt.Entities.Core;
using TrackIt.Tests.Config;
using TrackIt.Entities;
using System.Net;

namespace TrackIt.Tests.Integration.Auth;

public class SignUpTests : TrackItSetup
{
  public SignUpTests (TrackItWebApplication fixture) : base (fixture)
  {
    DateTimeProvider.Set(() => DateTime.Parse("2024-07-20T00:00:00"));
    GenerateTicketCode.Set(() => "123456");
  }
  
  [Fact]
  public async Task ShouldSignUpAndSendEmail ()
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
    
    var ticket = await _db.Ticket
      .FirstOrDefaultAsync(t => t.UserId == created.Id);
    
    Assert.NotNull(created.Email);
    Assert.NotNull(created.Password);
    Assert.Equal("gvitchenzo@gmail.com", created.Email.Value);
    Assert.True(Password.Verify("PasswordTest@1234", created.Password));
    
    Assert.NotNull(ticket);
    Assert.Equal(created.Id, ticket.UserId);
    Assert.Equal(created.Email.Value, ticket.ValidationObject);
    Assert.Equal(TicketType.EMAIL_VERIFICATION, ticket.Type);
    Assert.Equal(TicketSituation.OPEN, ticket.Situation);
    
    Assert.NotNull(_harness.Published.Select(p => p.MessageObject.GetType() == typeof(SendEmailVerificationEvent)).FirstOrDefault());
  }
  
  [Fact]
  public async Task ShouldThrowEmailAlreadyInUse ()
  {
    await CreateUser();
    
    var payload = new SignUpPayload(
      Email: "gvitchenzo@gmail.com",
      Password: "DiffPassword@1234"
    );

    var response = await _httpClient.PostAsync("/auth/sign-up", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Email already in use!", result.Message);
    Assert.Equal("EMAIL_ALREADY_IN_USE", result.Code);
    Assert.Equal(400, result.StatusCode);
  }

  [Fact]
  public async Task ShouldThrowUserAlreadyExists ()
  {
    await CreateUserWithEmailValidated();
    
    var payload = new SignUpPayload(
      Email: "gvitchenzo@gmail.com",
      Password: "PasswordTest@1234"
    );

    var response = await _httpClient.PostAsync("/auth/sign-up", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("User already exists!", result.Message);
    Assert.Equal("USER_ALREADY_EXISTS", result.Code);
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