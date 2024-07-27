using TrackIt.Entities.Core;
using TrackIt.Tests.Mocks.Contracts;

namespace TrackIt.Tests.Mocks.Entities;

public class TicketMock : Ticket, IMock<Ticket>
{
  public TicketMock ChangeUserId (Guid userId)
  {
    UserId = userId;

    return this;
  }

  public TicketMock ChangeCreatedAt (DateTime dateTime) 
  {
    CreatedAt = dateTime;

    return this;
  }

  public TicketMock ChangeType (TicketType type)
  {
    Type = type;

    return this;
  }

  public TicketMock ChangeValidationObject (string validationObject)
  {
    ValidationObject = validationObject;

    return this;
  }

  public TicketMock ChangeSituation (TicketSituation situation)
  {
    Situation = situation;

    return this;
  }

  public TicketMock ChangeCode (string code)
  {
    Code = code;

    return this;
  }
  
  public void Verify (Ticket expect, Ticket current)
  {
    Assert.Equal(expect.Id, current.Id);
    Assert.Equal(expect.UserId, current.UserId);
    Assert.Equal(expect.Code, current.Code);
    Assert.Equal(expect.ValidationObject, current.ValidationObject);
    Assert.Equal(expect.Type, current.Type);
    Assert.Equal(expect.Situation, current.Situation);
    Assert.Equal(expect.CreatedAt, current.CreatedAt);
  }
}