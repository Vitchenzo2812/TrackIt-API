﻿using TrackIt.Entities.Core;

namespace TrackIt.Infraestructure.Mailer.Models;

public class MailerSignUpRequestData : Data
{
  public required string Code { get; set; }
}