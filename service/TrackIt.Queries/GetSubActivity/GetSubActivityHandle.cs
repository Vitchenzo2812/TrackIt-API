using TrackIt.Entities.Repository;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Queries.GetSubActivity;

public class GetSubActivityHandle : IRequestHandler<GetSubActivityQuery, GetSubActivityResult>
{
  private readonly ISubActivityRepository _subActivityRepository;

  public GetSubActivityHandle (
    ISubActivityRepository subActivityRepository
  )
  {
    _subActivityRepository = subActivityRepository;
  }
  
  public async Task<GetSubActivityResult> Handle (GetSubActivityQuery request, CancellationToken cancellationToken)
  {
    var subActivity = await _subActivityRepository.FindById(request.Params.SubActivityId);

    if (subActivity is null)
      throw new NotFoundError("Sub Activity not found");

    return GetSubActivityResult.Build(subActivity);
  }
}