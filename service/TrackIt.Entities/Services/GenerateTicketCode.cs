namespace TrackIt.Entities.Services;

public class GenerateTicketCode
{
  private static Func<string> _GenerateTicketCodeFunc = () => GenerateDefault();
  
  public static string Code => _GenerateTicketCodeFunc();


  public static void Set(Func<string> GenerateTicketCodeFunc)
  {
    _GenerateTicketCodeFunc = GenerateTicketCodeFunc;
  }

  private static string GenerateDefault()
  {
    const int LENGTH = 6;

    Random random = new Random();
    var value = string.Empty;

    for (int i = 0; i < LENGTH; i++)
    {
      value += random.Next(0, 10).ToString();
    }

    return value;
  }
}