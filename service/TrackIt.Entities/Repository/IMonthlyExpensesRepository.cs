﻿using TrackIt.Entities.Expenses;

namespace TrackIt.Entities.Repository;

public interface IMonthlyExpensesRepository : IRepository<MonthlyExpenses>
{
  Task<MonthlyExpenses?> FindByDate (DateTime date);
  Task<List<MonthlyExpenses>> GetAllByUserId (Guid userId);
}