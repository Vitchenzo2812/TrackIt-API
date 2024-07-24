using TrackIt.Infraestructure.Extensions;
using TrackIt.Infraestructure.Web.Dto;
using TrackIt.Tests.Config.Builders;
using TrackIt.Tests.Mocks.Entities;
using TrackIt.Queries.Views;
using TrackIt.Tests.Config;
using TrackIt.Entities;
using System.Net;

namespace TrackIt.Tests.Integration.User;

public class GetUsersTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  private UserMock user1 { get; set; } 
  
  private UserMock user2 { get; set; } 
  
  private UserMock user3 { get; set; } 
  
  [Fact]
  public async Task ShouldReturnPaginationUsers ()
  {
    await CreateUsers();
    
    AddAuthorizationData(SessionBuilder.Build(await CreateAdminUser()));
    
    var response = await _httpClient.GetAsync("/user?page=1&perPage=2");
    var result = await response.ToData<PaginationView<List<UserResourceView>>>();
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    Assert.Equal(1, result.Page);
    Assert.Equal(2, result.Pages);
    Assert.Equal(2, result.Data.Count);

    VerifyUsers(result.Data);
  }

  [Fact]
  public async Task ShouldReturnPaginationUsersWithRecentlyOrder ()
  {
    await CreateUsers();
    
    AddAuthorizationData(SessionBuilder.Build(await CreateAdminUser()));
    
    var response = await _httpClient.GetAsync("/user?page=1&perPage=3&sort=RECENTLY");
    var result = await response.ToData<PaginationView<List<UserResourceView>>>();
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    Assert.Equal(1, result.Page);
    Assert.Equal(1, result.Pages);
    Assert.Equal(3, result.Data.Count);
    
    Assert.Equal(user2.Id, result.Data[0].Id);
    Assert.Equal(user1.Id, result.Data[1].Id);
    Assert.Equal(user3.Id, result.Data[2].Id);

    VerifyUsers(result.Data);
  }
    
  [Fact]
  public async Task ShouldReturnPaginationUsersWithOldOrder ()
  {
    await CreateUsers();
    
    AddAuthorizationData(SessionBuilder.Build(await CreateAdminUser()));
    
    var response = await _httpClient.GetAsync("/user?page=1&perPage=3&sort=OLD");
    var result = await response.ToData<PaginationView<List<UserResourceView>>>();
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    Assert.Equal(1, result.Page);
    Assert.Equal(1, result.Pages);
    Assert.Equal(3, result.Data.Count);
    
    Assert.Equal(user3.Id, result.Data[0].Id);
    Assert.Equal(user1.Id, result.Data[1].Id);
    Assert.Equal(user2.Id, result.Data[2].Id);

    VerifyUsers(result.Data);
  }
    
  [Fact]
  public async Task ShouldReturnForbiddenIfIsntAdmin ()
  {
    AddAuthorizationData(SessionBuilder.Build(await CreateUser()));
    
    var response = await _httpClient.GetAsync("/user?page=1&perPage=2");
    var result = await response.ToData<ErrorResponseDto>();
    
    Assert.Equal("Forbidden Error", result.Message);
    Assert.Equal("FORBIDDEN_ERROR", result.Code);
    Assert.Equal(403, result.StatusCode);
  }

  private void VerifyUsers (List<UserResourceView> givenUsers)
  {
    List<UserMock> users = [user1, user2, user3];

    foreach (var u in givenUsers)
    {
      var user = users.FirstOrDefault(_u => _u.Id == u.Id);
      
      Assert.NotNull(user);
      
      Assert.Equal(user.Id, u.Id);
      Assert.Equal(user.Name, u.Name);
      Assert.Equal(user.Email!.Value, u.Email);
      Assert.Equal(user.Hierarchy, u.Hierarchy);
      Assert.Equal(user.CreatedAt, u.CreatedAt);
    }
  }
  
  private async Task CreateUsers ()
  {
    user1 = new UserMock()
      .ChangeName("Jonas")
      .ChangeEmail("jonasDaSilva@gmail.com")
      .ChangePassword(Password.Create("JonasIncrivel_1264"))
      .ChangeCreatedAt(DateTime.Parse("2024-07-23T19:00:00"));

    user2 = new UserMock()
      .ChangeName("Lucas")
      .ChangeEmail("lucasOliveira@gmail.com")
      .ChangePassword(Password.Create("Incrivel@Jogador99"))
      .ChangeIncome(2000)
      .ChangeCreatedAt(DateTime.Parse("2024-07-23T19:15:00"));

    user3 = new UserMock()
      .ChangeName("Mario")
      .ChangeEmail("armario55@gmail.com")
      .ChangePassword(Password.Create("MarioAugusto_31092002"))
      .WithEmailValidated()
      .ChangeCreatedAt(DateTime.Parse("2024-07-22T20:00:00"));

    _db.User.Add(user1);
    _db.User.Add(user2);
    _db.User.Add(user3);
    await _db.SaveChangesAsync();
  }
}