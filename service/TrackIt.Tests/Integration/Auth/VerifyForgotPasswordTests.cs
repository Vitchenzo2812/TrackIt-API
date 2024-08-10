using Session = TrackIt.Infraestructure.Security.Models.Session;
using TrackIt.Commands.Auth.VerifyForgotPassword;
using TrackIt.Commands.Auth.ForgotPassword;
using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Web.Dto;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Mocks.Entities;
using TrackIt.Entities.Core;
using TrackIt.Tests.Config;
using System.Net;

namespace TrackIt.Tests.Integration.Auth;

public class VerifyForgotPasswordTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldVerifyCode ()
  {
    var user = await CreateUserWithEmailValidated();

    var payload = new ForgotPasswordPayload(
      Email: "gvitchenzo@gmail.com"
    );
      
    var response = await _httpClient.PostAsync("/auth/forgot-password", payload.ToJson());
    var result = await response.ToData<ForgotPasswordResponse>();
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    Assert.Equal(user.Id, result.UserId);

    var ticket = await _db.Ticket.FirstOrDefaultAsync(
      t => t.UserId == user.Id &&
           t.Type == TicketType.RESET_PASSWORD &&
           t.Situation == TicketSituation.OPEN
    );
    
    Assert.NotNull(ticket);

    var payload1 = new VerifyForgotPasswordPayload(
      UserId: result.UserId,
      Code: ticket.Code
    );
    
    var response1 = await _httpClient.PostAsync("/auth/forgot-password/verify", payload1.ToJson());
    var result1 = await response1.ToData<Session>();
    
    Assert.Equal(HttpStatusCode.OK, response1.StatusCode);
    
    Assert.NotNull(result1.Token);
    Assert.NotNull(result1.RefreshToken);
    Assert.Equal(user.Id, result1.Id);
    Assert.Equal(user.Email!.Value, result1.Email);
    Assert.Equal(user.Hierarchy, result1.Hierarchy);
    Assert.Equal(user.Income, result1.Income);
    Assert.Equal(user.Name, result1.Name);
  }

  [Fact]
  public async Task ShouldThrowUserNotFound ()
  {
    var payload = new VerifyForgotPasswordPayload(
      UserId: Guid.NewGuid(),
      Code: "123456"
    );

    var response = await _httpClient.PostAsync("/auth/forgot-password/verify", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("User not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }
  
  [Fact]
  public async Task ShouldThrowTicketNotFound ()
  {
    var user = await CreateUser();
    
    var payload = new VerifyForgotPasswordPayload(
      UserId: user.Id,
      Code: "123456"
    );

    var response = await _httpClient.PostAsync("/auth/forgot-password/verify", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Ticket not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }

  [Fact]
  public async Task ShouldThrowTicketCannotBeClosed ()
  {
    var user = await CreateUser();
    await CreateTicket(user);

    var payload = new VerifyForgotPasswordPayload(
      UserId: user.Id,
      Code: "some_code"
    );

    var response = await _httpClient.PostAsync("/auth/forgot-password/verify", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Invalid given code", result.Message);
    Assert.Equal("TICKET_CANNOT_BE_CLOSED", result.Code);
    Assert.Equal(400, result.StatusCode);
  }
  
  private async Task CreateTicket (UserMock user)
  {
    var ticket = new TicketMock()
      .ChangeCode("123456")
      .ChangeSituation(TicketSituation.OPEN)
      .ChangeType(TicketType.RESET_PASSWORD)
      .ChangeUserId(user.Id)
      .ChangeCreatedAt(DateTime.Parse("2024-07-27T00:00:00"))
      .ChangeValidationObject(user.Email!.Value);

    await _db.Ticket.AddAsync(ticket);
    await _db.SaveChangesAsync();
  }
}