using TrackIt.Entities;
using TrackIt.Tests.Mocks;

namespace TrackIt.Tests.Unit;

public class UserTests
{
  [Fact]
  public void ShouldCreateAnEmpty ()
  {
    var user = new UserMock();
    
    Assert.Null(user.Name);
    Assert.Null(user.Income);
    Assert.Null(user.Email);
    Assert.Null(user.Password);
  }
  
  [Fact]
  public void ShouldCreateWithSomeValues ()
  {
    var password = Password.Create("PasswordTest@1234");
    
    var user = new UserMock()
      .ChangeName("Guilherme")
      .ChangeIncome(2000)
      .ChangeEmail("gvitchenzo@gmail.com")
      .ChangePassword(password);
    
    Assert.Equal("Guilherme", user.Name);
    Assert.Equal(2000, user.Income);
    Assert.Equal("gvitchenzo@gmail.com", user.Email.Value);
    Assert.True(Password.Verify("PasswordTest@1234", password));
  }
}