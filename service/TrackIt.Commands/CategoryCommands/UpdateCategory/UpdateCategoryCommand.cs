using TrackIt.Entities.Core;

namespace TrackIt.Commands.CategoryCommands.UpdateCategory;

public class UpdateCategoryCommand(Guid aggregateId, UpdateCategoryPayload payload, Session? session = null)
  : Command<Guid, UpdateCategoryPayload>(aggregateId, payload, session);