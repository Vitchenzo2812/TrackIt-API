using TrackIt.Entities.Core;

namespace TrackIt.Queries.GetPaymentFormats;

public class GetPaymentFormatsQuery (Session? session = null)
  : Query<object, List<GetPaymentFormatsResult>>(null, session);