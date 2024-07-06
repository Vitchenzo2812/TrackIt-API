using TrackIt.Entities;
using TrackIt.Entities.Errors;

namespace TrackIt.Tests.Unit;

public class EmailTests
{
  [Fact]
  public void ShouldValidateEmail ()
  {
    Email email = Email.FromAddress("gvitchenzo@gmail.com");
    
    Assert.Equal("gvitchenzo@gmail.com", email.Value);
  }
  
  [Fact]
  public void ShouldThrowArgumentExceptionIfEmailIsNullOrEmpty ()
  {
    Assert.Throws<ArgumentException>(() => Email.FromAddress(""));
  }

  [Fact]
  public void ShouldThrowInvalidEmailError ()
  {
    Assert.Throws<InvalidEmailError>(() => Email.FromAddress("123456"));
  }
}