using TrackIt.Entities;
using TrackIt.Entities.Errors;

namespace TrackIt.Tests.Unit;

public class PasswordTests
{
  [Fact]
  public void ShouldCreatePassword ()
  {
    string givenPassword = "PasswordTest@1234";

    Password password = Password.Create(givenPassword);
    
    Assert.True(Password.Verify(givenPassword, password));
  }
  
  [Fact]
  public void ShouldReturnFalseIfPasswordDoesntMatch ()
  {
    string givenPassword = "PasswordTest@1234";

    Password password = Password.Create(givenPassword);
    
    Assert.False(Password.Verify("Diff_Password", password));
  }

  [Fact]
  public void ShouldThrowErrorIfPasswordDontHasMinCharacters ()
  {
    var exception = Assert.Throws<InvalidPasswordError>(() => Password.Create("test"));
    Assert.Equal("Password must be 8 characters", exception.Message);
  }
  
  [Fact]
  public void ShouldThrowErrorIfPasswordDontHasUpperCaseLetter ()
  {
    var exception = Assert.Throws<InvalidPasswordError>(() => Password.Create("password"));
    Assert.Equal("Password must be contains upper case letter", exception.Message);
  }
  
  [Fact]
  public void ShouldThrowErrorIfPasswordDontHasLowerCaseLetter ()
  {
    var exception = Assert.Throws<InvalidPasswordError>(() => Password.Create("PASSWORD"));
    Assert.Equal("Password must be contains lower case letter", exception.Message);
  }
  
  [Fact]
  public void ShouldThrowErrorIfPasswordDontHasDigits ()
  {
    var exception = Assert.Throws<InvalidPasswordError>(() => Password.Create("Password"));
    Assert.Equal("Password must be contains digits", exception.Message);
  }
  
  [Fact]
  public void ShouldThrowErrorIfPasswordDontHasSpecialCharacter ()
  {
    var exception = Assert.Throws<InvalidPasswordError>(() => Password.Create("Password1234"));
    Assert.Equal("Password must be contains a special character", exception.Message);
  }
}