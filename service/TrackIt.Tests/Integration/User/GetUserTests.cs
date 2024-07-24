using System.Net;
using TrackIt.Entities;
using TrackIt.Entities.Services;
using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Web.Dto;
using TrackIt.Queries.Views;
using TrackIt.Tests.Config;
using TrackIt.Tests.Config.Builders;
using TrackIt.Tests.Mocks.Entities;

namespace TrackIt.Tests.Integration.User;

public class GetUserTests : TrackItSetup
{
  public GetUserTests (TrackItWebApplication fixture) : base (fixture)
  {
    DateTimeProvider.Set(() => DateTime.Parse("2024-07-23T00:00:00"));
  }
  
  [Fact]
  public async Task ShouldReturnUser ()
  {
    var user = await CreateUser();
    
    AddAuthorizationData(SessionBuilder.Build(user));

    var response = await _httpClient.GetAsync($"/user/{user.Id}");
    var result = await response.ToData<UserView>();
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    
    Assert.Equal(result.Id, user.Id);
    Assert.Equal(result.Name, user.Name);
    Assert.Equal(result.Income, user.Income);
    Assert.Equal(result.Email, user.Email!.Value);
    Assert.Equal(result.Hierarchy, user.Hierarchy);
    Assert.Equal(result.CreatedAt, user.CreatedAt);
    Assert.Equal(result.PasswordMask, user.Password!.MaskPassword());
  }

  [Fact]
  public async Task ShouldThrowForbidden ()
  {
    var user = await CreateUser();
    
    AddAuthorizationData(SessionBuilder.Build(UserMock.Build(Password.Create("Password@1234"))));
    
    var response = await _httpClient.GetAsync($"/user/{user.Id}");
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Forbidden Error", result.Message);
    Assert.Equal("FORBIDDEN_ERROR", result.Code);
    Assert.Equal(403, result.StatusCode);
  }

  [Fact]
  public async Task ShouldThrowUserNotFound ()
  {
    var user = await CreateUser();
    
    AddAuthorizationData(SessionBuilder.Build(user));

    var response = await _httpClient.GetAsync($"/user/{Guid.NewGuid()}");
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("User not found", result.Message);
    Assert.Equal("NOT_FOUND", result.Code);
    Assert.Equal(404, result.StatusCode);
  }

  [Fact]
  public async Task ShouldCanGetUserIfIsAdmin ()
  {
    var user = await CreateUser();
    
    AddAuthorizationData(SessionBuilder.Build(await CreateAdminUser()));

    var response = await _httpClient.GetAsync($"/user/{user.Id}");
    var result = await response.ToData<UserView>();
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    
    Assert.Equal(result.Id, user.Id);
    Assert.Equal(result.Name, user.Name);
    Assert.Equal(result.Income, user.Income);
    Assert.Equal(result.Email, user.Email!.Value);
    Assert.Equal(result.Hierarchy, user.Hierarchy);
    Assert.Equal(result.CreatedAt, user.CreatedAt);
    Assert.Equal(result.PasswordMask, user.Password!.MaskPassword());
  }
}