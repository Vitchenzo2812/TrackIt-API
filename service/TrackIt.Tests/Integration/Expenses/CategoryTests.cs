using TrackIt.Commands.CategoryCommands.CreateCategory;
using TrackIt.Infraestructure.Extensions;
using Microsoft.EntityFrameworkCore;
using TrackIt.Tests.Config.Builders;
using TrackIt.Entities.Expenses;
using TrackIt.Entities.Events;
using TrackIt.Tests.Config;
using System.Net;

namespace TrackIt.Tests.Integration.Expenses;

public class CategoryTests (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  private Category category1 { get; set; }
  private Category category2 { get; set; }
  
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

  [Fact]
  public async Task ShouldUpdateCategory ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateCategories();
    
    var payload = new CreateCategoryPayload(
      Title: "DIFF_CATEGORY_2",
      Description: "DIFF_CATEGORY_2_DESCRIPTION",
      Icon: "DIFF_ICON_2",
      IconColor: "DIFF_ICON_COLOR_2",
      BackgroundIconColor: "DIFF_BACKGROUND_ICON_COLOR_2"
    );

    var response = await _httpClient.PutAsync($"/category/{category2.Id}", payload.ToJson());
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var updated = await _db.Categories.FirstOrDefaultAsync(x => x.Id == category2.Id);
    
    Assert.NotNull(updated);
    Assert.Equal(payload.Title, updated.Title);
    Assert.Equal(payload.Description, updated.Description);
  }

  [Fact]
  public async Task ShouldDeleteCategory ()
  {
    var user = await CreateUserWithEmailValidated();
    AddAuthorizationData(SessionBuilder.Build(user));

    await CreateCategories();

    var response = await _httpClient.DeleteAsync($"/category/{category1.Id}");
    
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var deleted = await _db.Categories.FirstOrDefaultAsync(x => x.Id == category1.Id);
    
    Assert.Null(deleted);
  }
  
  private async Task CreateCategories ()
  {
    category1 = Category.Create()
      .WithTitle("CATEGORY_1")
      .WithDescription("CATEGORY_1_DESCRIPTION");
    
    var config1 = CategoryConfig.Create()
      .WithIcon("ICON_1")
      .WithIconColor("ICON_COLOR_1")
      .WithBackgroundIconColor("BACKGROUND_ICON_COLOR_1")
      .AssignToCategory(category1.Id);
    
    category2 = Category.Create()
      .WithTitle("CATEGORY_2")
      .WithDescription("CATEGORY_2_DESCRIPTION");

    var config2 = CategoryConfig.Create()
      .WithIcon("ICON_2")
      .WithIconColor("ICON_COLOR_2")
      .WithBackgroundIconColor("BACKGROUND_ICON_COLOR_2")
      .AssignToCategory(category2.Id);

    _db.Categories.AddRange([category1, category2]);
    _db.CategoryConfigs.AddRange([config1, config2]);
    await _db.SaveChangesAsync();
  }
}