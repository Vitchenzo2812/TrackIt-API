namespace TrackIt.Entities.Errors;

public class InvalidAmountValueError : ApplicationError
{
  public InvalidAmountValueError () : base(400, "INVALID_AMOUNT_VALUE", "Invalid amount value")
  {
  }
}