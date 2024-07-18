using TrackIt.Entities.Core;

namespace TrackIt.Infraestructure.Mailer.Models;

public record MailRequest (
  string[] Addresses,
  
  string Template,
  
  string Subject,
  
  Data? Data = null
);