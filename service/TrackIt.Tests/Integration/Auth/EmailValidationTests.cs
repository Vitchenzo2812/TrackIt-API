using Microsoft.EntityFrameworkCore;
using TrackIt.Commands.Auth.EmailValidation;
using TrackIt.Commands.Auth.SignUp;
using TrackIt.Entities.Core;
using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Web.Dto;
using TrackIt.Tests.Config;
using TrackIt.Tests.Mocks.Entities;
using Session = TrackIt.Infraestructure.Security.Models.Session;

namespace TrackIt.Tests.Integration.Auth;

public class EmailValidationTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldValidateEmail ()
  {
    var signUpPayload = new SignUpPayload(
      Email: "gvitchenzo@gmail.com",
      Password: "PasswordTest@1234"
    );
    
    var responseSignUp = await _httpClient.PostAsync("/auth/sign-up", signUpPayload.ToJson());
    var resultSignUp = await responseSignUp.ToData<SignUpResponse>();

    var ticket = await _db.Ticket.FirstOrDefaultAsync(t => t.UserId == resultSignUp.UserId);
    
    Assert.NotNull(ticket);
    
    var emailValidationPayload = new EmailValidationPayload(
      UserId: resultSignUp.UserId,
      Code: ticket.Code
    );
    
    var response = await _httpClient.PostAsync("/auth/email-verification", emailValidationPayload.ToJson());
    var result = await response.ToData<Session>();

    var userValidated = await _db.User.FirstOrDefaultAsync(u => u.Id == result.Id); 
    
    Assert.NotNull(userValidated);
    
    Assert.NotNull(result.Token);
    Assert.NotNull(result.RefreshToken);
    
    Assert.Null(result.Name);
    Assert.Null(result.Income);
    Assert.Equal(userValidated.Id, result.Id);
    Assert.Equal(userValidated.Email!.Value, result.Email);
    Assert.Equal(userValidated.Hierarchy, result.Hierarchy);
  }

  [Fact]
  public async Task ShouldThrowTicketNotFound ()
  {
    var emailValidationPayload = new EmailValidationPayload(
      UserId: Guid.NewGuid(),
      Code: "123456"
    );
    
    var response = await _httpClient.PostAsync("/auth/email-verification", emailValidationPayload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Ticket not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }

  [Fact]
  public async Task ShouldThrowTicketCannotBeClosedIfCodeIsWrong ()
  {
    var user = await CreateUser();
    await CreateTicket(user);
    
    var emailValidationPayload = new EmailValidationPayload(
      UserId: user.Id,
      Code: "some_code"
    );
    
    var response = await _httpClient.PostAsync("/auth/email-verification", emailValidationPayload.ToJson());
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
      .ChangeType(TicketType.EMAIL_VERIFICATION)
      .ChangeUserId(user.Id)
      .ChangeCreatedAt(DateTime.Parse("2024-07-27T00:00:00"))
      .ChangeValidationObject(user.Email!.Value);

    await _db.Ticket.AddAsync(ticket);
    await _db.SaveChangesAsync();
  }
}