﻿using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Expenses;
using TrackIt.Entities.Events;
using TrackIt.Tests.Config;

namespace TrackIt.Tests.Integration.Consumers;

public class UpdateCategoryConsumer (TrackItWebApplication fixture) : TrackItSetup (fixture)
{
  [Fact]
  public async Task ShouldUpdateCategoryConfig ()
  {
    var category = await CreateCategoryAndConfig();
    
    var icon = "DIFF_ICON";
    var iconColor = "DIFF_ICON_COLOR";
    var backgroundIconColor = "DIFF_BACKGROUND_ICON_COLOR";
      
    await _harness.Bus.Publish(
      new UpdateCategoryEvent(category.Id, icon, iconColor, backgroundIconColor)
    );
    
    var consumed = await _harness.Consumed.Any<UpdateCategoryEvent>();
    
    Assert.True(consumed);

    var configs = await _db.CategoryConfigs.ToListAsync();

    var updatedConfig = configs.Find(x => x.CategoryId == category.Id);
    
    Assert.NotNull(updatedConfig);
    Assert.Equal(category.Id, updatedConfig.CategoryId);
    Assert.Equal(icon, updatedConfig.Icon);
    Assert.Equal(iconColor, updatedConfig.IconColor);
    Assert.Equal(backgroundIconColor, updatedConfig.BackgroundIconColor);
  }
  
  private async Task<Category> CreateCategoryAndConfig ()
  {
    var category = Category.Create()
      .WithTitle("CATEGORY_1")
      .WithDescription("CATEGORY_1_DESCRIPTION");

    var config = CategoryConfig.Create()
      .WithIcon("ICON")
      .WithIconColor("ICON_COLOR")
      .WithBackgroundIconColor("BACKGROUND_ICON_COLOR")
      .AssignToCategory(category.Id);
    
    _db.Categories.Add(category);
    _db.CategoryConfigs.Add(config);
    
    await _db.SaveChangesAsync();
    
    return category;
  }
}