using TrackIt.Commands.CategoryCommands.CreateCategory;
using TrackIt.Infraestructure.Extensions;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Config.Builders;
using TrackIt.Entities.Events;
using TrackIt.Tests.Config;
using System.Net;

namespace TrackIt.Tests.Integration.Expenses;

public class CategoryTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldCreateCategory ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    var payload = new CreateCategoryPayload(
      Title: "CATEGORY_1",
      Description: "CATEGORY_1_DESCRIPTION",
      Icon: "ICON",
      IconColor: "ICON_COLOR",
      BackgroundIconColor: "BACKGROUND_ICON_COLOR"
    );
    
    var response = await _httpClient.PostAsync("/category", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    
    var categories = await _db.Categories.ToListAsync();

    var createdCategory = categories.Find(x => x.Title == payload.Title);
    Assert.NotNull(createdCategory);
    
    Assert.Equal(payload.Title, createdCategory.Title);
    Assert.Equal(payload.Description, createdCategory.Description);
    
    Assert.NotNull(_harness.Published.Select(p => p.MessageObject.GetType() == typeof(CreateCategoryEvent)).FirstOrDefault());
  }
}