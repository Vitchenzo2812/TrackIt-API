using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Core;

namespace TrackIt.Infraestructure.Repository;

public class TicketRepository : ITicketRepository
{
  private readonly TrackItDbContext _db;

  public TicketRepository (TrackItDbContext db)
  {
    _db = db;
  }
  
  public async Task<Ticket?> FindById (Guid aggregateId)
  {
    return (await _db.Ticket.FirstOrDefaultAsync(t => t.Id == aggregateId));
  }

  public void Save (Ticket aggregate)
  {
    _db.Ticket.Add(aggregate);
  }

  public void Delete (Ticket aggregate)
  {
    _db.Ticket.Remove(aggregate);
  }
  
  public async Task<Ticket?> FindLastWithTypeAndSituation (Guid userId, TicketType type, TicketSituation situation)
  {
    return (await _db.Ticket
      .OrderByDescending(t => t.CreatedAt)
      .FirstOrDefaultAsync(t => t.Type == type && t.Situation == situation && t.UserId == userId));
  }
}