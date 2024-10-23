using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Events;
using TrackIt.Entities.Expenses;
using TrackIt.Tests.Config;

namespace TrackIt.Tests.Integration.Consumers;

public class CreateCategoryConsumer (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldCreateCategoryConfig ()
  {
    var category = await CreateCategory();

    var icon = "ICON";
    var iconColor = "ICON_COLOR";
    var backgroundIconColor = "BACKGROUND_ICON_COLOR";
      
    await _harness.Bus.Publish(
      new CreateCategoryEvent(category.Id, icon, iconColor, backgroundIconColor)
    );
    
    var consumed = await _harness.Consumed.Any<CreateCategoryEvent>();
    
    Assert.True(consumed);

    var configs = await _db.CategoryConfigs.ToListAsync();

    var createdConfig = configs.Find(x => x.CategoryId == category.Id);
    
    Assert.NotNull(createdConfig);
    Assert.Equal(category.Id, createdConfig.CategoryId);
    Assert.Equal(icon, createdConfig.Icon);
    Assert.Equal(iconColor, createdConfig.IconColor);
    Assert.Equal(backgroundIconColor, createdConfig.BackgroundIconColor);
  }

  private async Task<Category> CreateCategory ()
  {
    var category = Category.Create()
      .WithTitle("CATEGORY_1")
      .WithDescription("CATEGORY_1_DESCRIPTION");

    _db.Categories.Add(category);
    await _db.SaveChangesAsync();
    
    return category;
  }
}