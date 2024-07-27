using TrackIt.Entities.Core;
using TrackIt.Entities.Errors;
using TrackIt.Tests.Mocks.Entities;
using TrackIt.Entities.Services;

namespace TrackIt.Tests.Unit;

public class TicketTests
{
  public TicketTests ()
  {
    DateTimeProvider.Set(() => DateTime.Parse("2024-07-21T00:00:00"));
    GenerateTicketCode.Set(() => "123456");
  }
  
  [Fact]
  public void ShouldCreateTicket ()
  { 
    var ticket = new TicketMock();
    
    Assert.Null(ticket.ValidationObject);
    Assert.Equal(TicketSituation.OPEN , ticket.Situation);
    Assert.Equal(DateTime.Parse("2024-07-21T00:00:00"), ticket.CreatedAt);
    Assert.Equal("123456", ticket.Code);
  }

  [Fact]
  public void ShouldThrowTicketCannotCancelled ()
  {
    var ticket = new TicketMock().ChangeCreatedAt(DateTime.Parse("2024-07-20T23:59:00"));

    var exception = Assert.Throws<TicketCannotBeCancelledError>(() => ticket.Cancel());
    
    Assert.Equal("Min time to request other ticket is 2 minutes", exception.Message);
  }

  [Fact]
  public void ShouldReturnFalseIfResending ()
  {
    var ticket = new TicketMock().ChangeCreatedAt(DateTime.Parse("2024-07-21T00:00:25"));
    
    Assert.False(ticket.CanReSend());
  }
  
  [Fact]
  public void ShouldThrowErrorDifferentCode ()
  {
    var ticket = new TicketMock();

    var exception = Assert.Throws<TicketCannotBeClosedError>(() => ticket.Close("code"));
    
    Assert.Equal("Invalid given code", exception.Message);
  }

  [Fact]
  public void ShouldCancelTicket ()
  {
    var ticket = new TicketMock().ChangeCreatedAt(DateTime.Parse("2024-07-20T23:55:00"));
    
    ticket.Cancel();
    
    Assert.Equal(TicketSituation.CANCELLED, ticket.Situation);
  }

  [Fact]
  public void ShouldReSendTicket ()
  {
    var ticket = new TicketMock().ChangeCreatedAt(DateTime.Parse("2024-07-20T23:59:00"));
    
    Assert.True(ticket.CanReSend());
  }

  [Fact]
  public void ShouldCloseTicket ()
  {
    var ticket = new TicketMock();
    
    ticket.Close("123456");
    
    Assert.Equal(TicketSituation.CLOSED, ticket.Situation);
  }
}