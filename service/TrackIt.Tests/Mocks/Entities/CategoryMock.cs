using TrackIt.Queries.GetCategories;
using TrackIt.Queries.GetCategory;
using TrackIt.Entities.Expenses;

namespace TrackIt.Tests.Mocks.Entities;

public class CategoryMock : Category
{
  public static void Verify (GetCategoryResult expect, Category current, CategoryConfig config)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.Title, current.Title);
    Assert.Equal(expect.Description, current.Description);
    Assert.Equal(expect.Icon, config.Icon);
    Assert.Equal(expect.IconColor, config.IconColor);
    Assert.Equal(expect.BackgroundIconColor, config.BackgroundIconColor);
  }
  
  public static void Verify (GetCategoriesResult expect, Category current, CategoryConfig config)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.Title, current.Title);
    Assert.Equal(expect.Description, current.Description);
    Assert.Equal(expect.Icon, config.Icon);
    Assert.Equal(expect.IconColor, config.IconColor);
    Assert.Equal(expect.BackgroundIconColor, config.BackgroundIconColor);
  }
}