using TrackIt.Entities.Core;

namespace TrackIt.Entities.Repository;

public interface ITicketRepository : IRepository<Ticket>
{
  Task<Ticket?> FindLastWithTypeAndSituation (Guid userId, TicketType type, TicketSituation situation);
}