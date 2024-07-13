using Microsoft.AspNetCore.Mvc;
using TrackIt.Entities.Core;

namespace TrackIt.Infraestructure.Web.Controller;

public class BaseController : ControllerBase
{
  protected Session SessionFromHeaders ()
  {
    var id = new Guid(HttpContext.Request.Headers["Id"].ToString());
    
    return new Session
    {
      Id = id,
      
      Email = HttpContext.Request.Headers["Email"].ToString(),
    };
  }
}