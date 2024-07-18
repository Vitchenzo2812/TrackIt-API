using TrackIt.Infraestructure.Mailer.Contracts;
using TrackIt.Infraestructure.Mailer.Models;
using Microsoft.AspNetCore.Hosting;
using Amazon.SimpleEmail.Model;
using TrackIt.Entities.Errors;
using Amazon.SimpleEmail;

namespace TrackIt.Infraestructure.Mailer;

public class MailerService : IMailerService
{
  private readonly IWebHostEnvironment _env;

  public MailerService (IWebHostEnvironment env)
  {
    _env = env;
  }
  
  public async Task Send (MailRequest request)
  {
    string? accessKeyId = Environment.GetEnvironmentVariable("MAIL_ACCESS_KEY_ID");
    string? secretAccessKey = Environment.GetEnvironmentVariable("MAIL_SECRET_ACCESS_KEY");
    string? region = Environment.GetEnvironmentVariable("MAIL_REGION");
    string? originAddress = Environment.GetEnvironmentVariable("MAIL_ORIGIN_ADDRESS");

    var client = new AmazonSimpleEmailServiceClient(accessKeyId, secretAccessKey, Amazon.RegionEndpoint.GetBySystemName(region));

    StreamReader reader = File.OpenText(
      Path.Combine(_env.WebRootPath, "Emails", "Templates", request.Template)
    );

    var html = await reader.ReadToEndAsync();

    if (string.IsNullOrEmpty(html))
      throw new InternalServerError("Failed to load template");

    if (request.Data is not null)
    {
      foreach (string key in request.Data.Keys)
      {
        html = html.Replace($"{{{{{key}}}}}", $"{request.Data.GetValueFromKey(key)}");
      }
    }

    var message = new SendEmailRequest
    {
      Source = originAddress,
      Destination = new Destination { ToAddresses = [..request.Addresses] },
      Message = new Message
      {
        Subject = new Content(request.Subject),
        Body = new Body
        {
          Html = new Content
          {
            Charset = "UTF-8",
            Data = html
          }
        }
      }
    };

    try
    {
      await client.SendEmailAsync(message);
    }
    catch (Exception e)
    {
      throw new InternalServerError(e.Message);
    }
  }
}