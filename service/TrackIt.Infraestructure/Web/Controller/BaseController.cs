using Microsoft.AspNetCore.Mvc;
using TrackIt.Entities.Core;

namespace TrackIt.Infraestructure.Web.Controller;

[ApiController]
public class BaseController : ControllerBase
{
  protected Session SessionFromHeaders ()
  {
    var id = new Guid(HttpContext.Request.Headers["Id"].ToString());
    var income = HttpContext.Request.Headers["Income"].ToString();

    double.TryParse(income, out var incomeConverted);
    
    return new Session
    {
      Id = id,
      
      Name = HttpContext.Request.Headers["Name"].ToString(),
      
      Email = HttpContext.Request.Headers["Email"].ToString(),
      
      Income = incomeConverted
    };
  }
}