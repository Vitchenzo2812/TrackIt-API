using TrackIt.Infraestructure.Repository.Contracts;
using TrackIt.Infraestructure.Database;
using Microsoft.EntityFrameworkCore;
using TrackIt.Entities;

namespace TrackIt.Infraestructure.Repository;

public class ExpenseRepository : IExpenseRepository
{
  private readonly TrackItDbContext _db;

  public ExpenseRepository (TrackItDbContext db)
  {
    _db = db;
  }
  
  public async Task<Expense?> FindById (Guid aggregateId)
  {
    return await _db.Expense
      .AsTracking()
      .FirstOrDefaultAsync(e => e.Id == aggregateId);
  }

  public void Save (Expense aggregate)
  {
    _db.Expense.Add(aggregate);
  }

  public void Delete (Expense aggregate)
  {
    _db.Expense.Remove(aggregate);
  }
}