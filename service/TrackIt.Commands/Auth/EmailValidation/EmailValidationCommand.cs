using Session = TrackIt.Infraestructure.Security.Models.Session;
using TrackIt.Entities.Core;

namespace TrackIt.Commands.Auth.EmailValidation;

public class EmailValidationCommand (EmailValidationPayload payload)
  : Command<object, EmailValidationPayload, Session>(null, payload);