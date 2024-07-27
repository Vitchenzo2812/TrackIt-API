using TrackIt.Commands.Auth.ForgotPassword;
using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Web.Dto;
using TrackIt.Entities.Events;
using TrackIt.Tests.Config;
using System.Net;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Core;
using TrackIt.Tests.Mocks.Entities;

namespace TrackIt.Tests.Integration.Auth;

public class ForgotPasswordTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldSendForgotPasswordEmail ()
  {
    var user = await CreateUser();
    
    var payload = new ForgotPasswordPayload(
      Email: "gvitchenzo@gmail.com"
    );

    var response = await _httpClient.PostAsync("/auth/forgot-password", payload.ToJson());
    var result = await response.ToData<ForgotPasswordResponse>();
      
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    Assert.Equal(user.Id, result.UserId);
    
    Assert.Equal(1, await _db.Ticket.CountAsync());
    
    Assert.NotNull(_harness.Published.Select(p => p.MessageObject.GetType() == typeof(ForgotPasswordEvent)).FirstOrDefault());
  }
  
  [Fact]
  public async Task ShouldThrowUserNotFound ()
  {
    var payload = new ForgotPasswordPayload(
      Email: "gvitchenzo@gmail.com"
    );

    var response = await _httpClient.PostAsync("/auth/forgot-password", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("User not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }
}