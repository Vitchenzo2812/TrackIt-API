using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities;

namespace TrackIt.Infraestructure.Repository;

public class MonthlyExpensesRepository : IMonthlyExpensesRepository
{
  private readonly TrackItDbContext _db;

  public MonthlyExpensesRepository (TrackItDbContext db)
  {
    _db = db;
  }
  
  public async Task<MonthlyExpenses?> FindById (Guid aggregateId)
  {
    return await _db.MonthlyExpenses
      .AsTracking()
      .Include(m => m.Expenses)
      .FirstOrDefaultAsync(m => m.Id == aggregateId);
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