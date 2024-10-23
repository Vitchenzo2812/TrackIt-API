using TrackIt.Entities.Core;

namespace TrackIt.Commands.CategoryCommands.DeleteCategory;

public class DeleteCategoryCommand(Guid aggregateId, Session? session = null)
  : Command<Guid, object>(aggregateId, null, session);