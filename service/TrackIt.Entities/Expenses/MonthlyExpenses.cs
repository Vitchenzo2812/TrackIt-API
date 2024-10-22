using TrackIt.Entities.Core;

namespace TrackIt.Entities.Expenses;

public class MonthlyExpenses : Entity
{
  public required Guid UserId { get; set; }
  public required string Title { get; set; }
  public required DateTime Date { get; set; }
  public required int? Limit { get; set; }
  private static readonly List<string> MonthNames = [
    "Janeiro",
    "Fevereiro",
    "Março",
    "Abril",
    "Maio",
    "Junho",
    "Julho",
    "Agosto",
    "Setembro",
    "Outubro",
    "Novembro",
    "Dezembro"
  ];
  
  public static MonthlyExpenses Create (Guid userId, DateTime date)
  {
    return new MonthlyExpenses
    {
      UserId = userId,
      Title = $"{MonthNames[date.Month - 1]} de {date.Year}",
      Date = date,
      Limit = null
    };
  }
}