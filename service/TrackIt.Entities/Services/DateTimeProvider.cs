namespace TrackIt.Entities.Services;

public class DateTimeProvider
{
  private static Func<DateTime> _dateTimeNowFunc = () => DateTime.Now;

  public static DateTime Now => _dateTimeNowFunc();
  
  public static void Set(Func<DateTime> dateTimeNowFunc)
  {
    _dateTimeNowFunc = dateTimeNowFunc;
  }
}