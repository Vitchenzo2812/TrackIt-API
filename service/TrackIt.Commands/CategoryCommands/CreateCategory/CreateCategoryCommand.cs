using TrackIt.Entities.Core;

namespace TrackIt.Commands.CategoryCommands.CreateCategory;

public class CreateCategoryCommand (CreateCategoryPayload payload, Session? session = null)
  : Command<CreateCategoryPayload>(payload, session);