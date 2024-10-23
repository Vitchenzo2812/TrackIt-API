using System.ComponentModel;

namespace TrackIt.Entities.Expenses;

public enum PaymentFormatKey
{
  [Description("CREDIT_CARD")]
  CREDIT_CARD,
  
  [Description("DEBIT_CARD")]
  DEBIT_CARD,
  
  [Description("PAYMENT_SLIP")]
  PAYMENT_SLIP,
  
  [Description("MONEY")]
  MONEY,
  
  [Description("PIX")]
  PIX,
  
  [Description("TICKET")]
  TICKET
}