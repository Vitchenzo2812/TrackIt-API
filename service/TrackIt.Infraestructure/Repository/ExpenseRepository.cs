using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities.Repository;
using TrackIt.Entities.Expenses;

namespace TrackIt.Infraestructure.Repository;

public class ExpenseRepository : IExpenseRepository
{
  private readonly TrackItDbContext _db;

  public ExpenseRepository (TrackItDbContext db) => _db = db; 
  
  public async Task<Expense?> FindById (Guid aggregateId)
  {
    return await _db.Expenses
      .AsTracking()
      .FirstOrDefaultAsync(x => x.Id == aggregateId);
  }

  public void Save (Expense aggregate)
  {
    _db.Expenses.Add(aggregate);
  }

  public void Delete (Expense aggregate)
  {
    _db.Expenses.Remove(aggregate);
  }
}