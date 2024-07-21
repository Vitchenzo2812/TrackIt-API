using TrackIt.Entities.Core;

namespace TrackIt.Infraestructure.Repository.Contracts;

public interface ITicketRepository : IRepository<Ticket>
{
  Task<Ticket?> FindLastWithTypeAndSituation (Guid userId, TicketType type, TicketSituation situation);
}