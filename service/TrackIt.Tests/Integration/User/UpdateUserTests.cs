using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Web.Dto;
using TrackIt.Tests.Config.Builders;
using TrackIt.Commands.UpdateUser;
using TrackIt.Queries.Views;
using TrackIt.Tests.Config;
using TrackIt.Tests.Mocks;
using TrackIt.Entities;
using System.Net;

namespace TrackIt.Tests.Integration.User;

public class UpdateUserTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  
  [Fact]
  public async Task ShouldUpdateUser ()
  {
    var user = await CreateUser();
    
    AddAuthorizationData(SessionBuilder.Build(user));
    
    var payload = new UpdateUserPayload(
      Name: "Luiz",
      Income: 2500
    );
    
    var response = await _httpClient.PutAsync($"/user/{user.Id}", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var updated = await (await _httpClient.GetAsync($"/user/{user.Id}")).ToData<UserView>();
    
    Assert.NotNull(updated);
    Assert.Equal("Luiz", updated.Name);
    Assert.Equal(2500, updated.Income);
  }

  [Fact]
  public async Task ShouldThrowUserNotFound ()
  {
    var user = UserMock.Build(Password.Create("PasswordTest@1234"));
    
    AddAuthorizationData(SessionBuilder.Build(user));
    
    var payload = new UpdateUserPayload(
      Name: "Luiz",
      Income: 2500
    );
    
    var response = await _httpClient.PutAsync($"/user/{user.Id}", payload.ToJson());
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("User not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  } 
}