using TrackIt.Entities;
using TrackIt.Tests.Mocks.Contracts;

namespace TrackIt.Tests.Mocks;

public class UserMock : User, IMock<User>
{
  public static UserMock Build (Password password)
  {
    return new UserMock
    {
      Name = "Guilherme",
      Income = 2000,
      Email = Email.FromAddress("gvitchenzo@gmail.com"),
      Password = password
    };
  }
  
  public UserMock ChangeName (string name)
  {
    Name = name;

    return this;
  }

  public UserMock ChangeIncome (double income)
  {
    Income = income;

    return this;
  }

  public UserMock ChangeEmail (string email)
  {
    Email = Email.FromAddress(email);

    return this;
  }

  public UserMock ChangePassword (Password password)
  {
    Password = password;

    return this;
  }

  public UserMock MakeAdministrator ()
  {
    Hierarchy = Hierarchy.ADMIN;

    return this;
  }
  
  public void Verify (User expect, User current)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.Name, current.Name);
    Assert.Equal(expect.Email.Value, current.Email.Value);
    Assert.Equal(expect.Password.Hash, current.Password.Hash);
    Assert.Equal(expect.Income, current.Income);
  }
}