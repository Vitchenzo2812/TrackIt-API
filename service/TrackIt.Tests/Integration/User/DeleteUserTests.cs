using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Web.Dto;
using TrackIt.Tests.Config.Builders;
using TrackIt.Tests.Mocks.Entities;
using TrackIt.Tests.Config;
using TrackIt.Entities;
using System.Net;

namespace TrackIt.Tests.Integration.User;

public class DeleteUserTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldDeleteUser ()
  {
    var user = await CreateUser();
    AddAuthorizationData(SessionBuilder.Build(user));

    var responseDelete = await _httpClient.DeleteAsync($"/user/{user.Id}");

    Assert.Equal(HttpStatusCode.OK, responseDelete.StatusCode);

    var responseGet = await _httpClient.GetAsync($"/user/{user.Id}");
    var result = await responseGet.ToData<ErrorResponseDto>();

    Assert.Equal("User not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }

  [Fact]
  public async Task ShouldDeleteIfUserIsAdmin ()
  {
    AddAuthorizationData(SessionBuilder.Build(await CreateAdminUser()));
    
    var user = await CreateUser();

    var responseDelete = await _httpClient.DeleteAsync($"/user/{user.Id}");

    Assert.Equal(HttpStatusCode.OK, responseDelete.StatusCode);

    var responseGet = await _httpClient.GetAsync($"/user/{user.Id}");
    var result = await responseGet.ToData<ErrorResponseDto>();

    Assert.Equal("User not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }

  [Fact]
  public async Task ShouldThrowUserNotFound ()
  {
    AddAuthorizationData(SessionBuilder.Build(await CreateUser()));
    
    var user = UserMock.Build(Password.Create("DiffPassword@1234"));

    var response = await _httpClient.DeleteAsync($"/user/{user.Id}");
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("User not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }
  
  [Fact]
  public async Task ShouldThrowSessionUserNotFound ()
  {
    AddAuthorizationData(SessionBuilder.Build(UserMock.Build(Password.Create("PasswordTest@1234"))));
    
    var user = await CreateUser();

    var response = await _httpClient.DeleteAsync($"/user/{user.Id}");
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Session user not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }
  
  [Fact]
  public async Task ShouldThrowForbiddenIfUserIsntAdmin ()
  {
    AddAuthorizationData(SessionBuilder.Build(await CreateUser()));
    
    var user = await CreateUser();

    var response = await _httpClient.DeleteAsync($"/user/{user.Id}");
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Forbidden Error", result.Message);
    Assert.Equal("FORBIDDEN_ERROR", result.Code);
    Assert.Equal(403, result.StatusCode);
  }
}