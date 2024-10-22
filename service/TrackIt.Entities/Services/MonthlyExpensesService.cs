using TrackIt.Entities.Repository;
using TrackIt.Entities.Expenses;

namespace TrackIt.Entities.Services;

public class MonthlyExpensesService
{
  private readonly IMonthlyExpensesRepository _monthlyExpensesRepository;
  
  public MonthlyExpensesService (
    IMonthlyExpensesRepository monthlyExpensesRepository
  )
  {
    _monthlyExpensesRepository = monthlyExpensesRepository;
  }

  public async Task AddExpenseToMonthlyGroup (Guid userId, Expense expense)
  {
    var existingMonthlyExpenses = await _monthlyExpensesRepository.FindByDate(expense.Date);

    if (existingMonthlyExpenses is not null)
    {
      expense.AssignToMonthly(existingMonthlyExpenses.Id);
      return;
    }
    
    var monthlyExpenses = MonthlyExpenses.Create(userId, expense.Date);
    _monthlyExpensesRepository.Save(monthlyExpenses);
    
    expense.AssignToMonthly(monthlyExpenses.Id);
  }
}