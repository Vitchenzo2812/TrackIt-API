using TrackIt.Infraestructure.Web.Swagger.Annotations;
using TrackIt.Infraestructure.Web.Controller;
using TrackIt.Queries.GetPaymentFormats;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace TrackIt.WebApi.Controllers;

[Tags("Payment Formats")]
[Route("payment-format")]
[ApiController]
public class PaymentFormats : BaseController
{
  private readonly IMediator _mediator;

  public PaymentFormats (IMediator mediator) => _mediator = mediator;

  [HttpGet]
  [SwaggerAuthorize]
  public async Task<List<GetPaymentFormatsResult>> Handle ()
  {
    return await _mediator.Send(new GetPaymentFormatsQuery(SessionFromHeaders()));
  }
}