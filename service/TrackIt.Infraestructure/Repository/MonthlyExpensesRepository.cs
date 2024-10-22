using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Expenses;

namespace TrackIt.Infraestructure.Repository;

public class MonthlyExpensesRepository : IMonthlyExpensesRepository
{
  private readonly TrackItDbContext _db;

  public MonthlyExpensesRepository (TrackItDbContext db) => _db = db; 
  
  public async Task<MonthlyExpenses?> FindById (Guid aggregateId)
  {
    return await _db.MonthlyExpenses
      .AsTracking()
      .FirstOrDefaultAsync(x => x.Id == aggregateId);
  }

  public async Task<MonthlyExpenses?> FindByDate (DateTime date)
  {
    return await _db.MonthlyExpenses
      .AsTracking()
      .FirstOrDefaultAsync(x => x.Date.Month == date.Month && x.Date.Year == date.Year);
  }

  public void Save (MonthlyExpenses aggregate)
  {
    _db.MonthlyExpenses.Add(aggregate);
  }

  public void Delete (MonthlyExpenses aggregate)
  {
    _db.MonthlyExpenses.Remove(aggregate);
  }
}