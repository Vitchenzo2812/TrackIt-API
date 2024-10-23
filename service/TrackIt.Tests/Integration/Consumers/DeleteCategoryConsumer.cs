using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Expenses;
using TrackIt.Entities.Events;
using TrackIt.Tests.Config;

namespace TrackIt.Tests.Integration.Consumers;

public class DeleteCategoryConsumer (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  private Category category1 { get; set; }
  private Category category2 { get; set; }
  private Category category3 { get; set; }
  
  [Fact]
  public async Task ShouldUpdateCategoryConfig ()
  {
    await CreateCategoryAndConfig();
    
    await _harness.Bus.Publish(
      new DeleteCategoryEvent(category2.Id)
    );
    
    var consumed = await _harness.Consumed.Any<DeleteCategoryEvent>();
    
    Assert.True(consumed);

    var configs = await _db.CategoryConfigs.ToListAsync();

    var deletedConfig = configs.Find(x => x.CategoryId == category2.Id);
    
    Assert.Null(deletedConfig);
  }
  
  private async Task CreateCategoryAndConfig ()
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
    
    category3 = Category.Create()
      .WithTitle("CATEGORY_3")
      .WithDescription("CATEGORY_3_DESCRIPTION");
    
    var config3 = CategoryConfig.Create()
      .WithIcon("ICON_3")
      .WithIconColor("ICON_COLOR_3")
      .WithBackgroundIconColor("BACKGROUND_ICON_COLOR_3")
      .AssignToCategory(category3.Id);
    
    _db.Categories.AddRange([category1, category2, category3]);
    _db.CategoryConfigs.AddRange([config1, config2, config3]);
    
    await _db.SaveChangesAsync();
  }
}