using System.ComponentModel;

namespace TrackIt.Entities;

public enum PaymentFormat
{
  [Description("DEBIT")]
  DEBIT,
  
  [Description("CREDIT")]
  CREDIT,
  
  [Description("CASH")]
  CASH
}