using TrackIt.Queries.Views.HomePage;
using TrackIt.Entities.Core;

namespace TrackIt.Queries.GetHomePageInfo;

public class GetHomePageInfoQuery(GetHomePageInfoParams @params, Session? session = null)
  : Query<GetHomePageInfoParams, HomePageView>(@params, session);