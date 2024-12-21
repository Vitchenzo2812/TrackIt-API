using TrackIt.Entities.Repository;
using TrackIt.Entities.Errors;
using MediatR;

namespace TrackIt.Queries.GetExpense;

public class GetExpenseHandle : IRequestHandler<GetExpenseQuery, GetExpenseResult>
{
  private readonly IExpenseRepository _expenseRepository;

  public GetExpenseHandle (
    IExpenseRepository expenseRepository
  )
  {
    _expenseRepository = expenseRepository;
  }
    
  public async Task<GetExpenseResult> Handle (GetExpenseQuery request, CancellationToken cancellationToken)
  {
    var expense = await _expenseRepository.FindById(request.Params.ExpenseId);

    if (expense is null)
      throw new NotFoundError("Expense not found");

    return GetExpenseResult.Build(expense);
  }
}