using TrackIt.Entities.Expenses;

namespace TrackIt.Tests.Unit;

public class CategoryTests
{
  [Fact]
  public void ShouldCreateAnEmpty ()
  {
    var category = Category.Create();
    var config = CategoryConfig.Create();
    
    Assert.Equal(string.Empty, category.Title);
    Assert.Equal(string.Empty, category.Description);
    
    Assert.Equal(string.Empty, config.Icon);
    Assert.Equal(string.Empty, config.IconColor);
    Assert.Equal(string.Empty, config.BackgroundIconColor);
  }

  [Fact]
  public void ShouldCreateWithSomeValues ()
  {
    var category = Category.Create()
      .WithTitle("CATEGORY_1")
      .WithDescription("CATEGORY_1_DESCRIPTION");

    var config = CategoryConfig.Create()
      .WithIcon("ICON")
      .WithIconColor("ICON_COLOR")
      .WithBackgroundIconColor("BACKGROUND_ICON_COLOR")
      .AssignToCategory(category.Id);
    
    Assert.Equal("CATEGORY_1", category.Title);
    Assert.Equal("CATEGORY_1_DESCRIPTION", category.Description);
    
    Assert.Equal("ICON", config.Icon);
    Assert.Equal("ICON_COLOR", config.IconColor);
    Assert.Equal("BACKGROUND_ICON_COLOR", config.BackgroundIconColor);
    Assert.Equal(category.Id, config.CategoryId);
  }
}