using TrackIt.Infraestructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using TrackIt.Entities.Core;
using TrackIt.Entities;

namespace TrackIt.Infraestructure.Web.Controller;

public class BaseController : ControllerBase
{
  protected Session SessionFromHeaders ()
  {
    var id = new Guid(HttpContext.Request.Headers["Id"].ToString());
    var name = HttpContext.Request.Headers["Name"].ToString();
    var email = HttpContext.Request.Headers["Email"].ToString();
    var income = HttpContext.Request.Headers["Income"].ToString();
    var hierarchy = HttpContext.Request.Headers["Hierarchy"].ToString().IntFromDescription<Hierarchy>();

    double.TryParse(income, out var incomeConverted);
    
    return new Session
    {
      Id = id,
      
      Name = name,
      
      Email = email,
      
      Hierarchy = hierarchy,
      
      Income = incomeConverted
    };
  }
}