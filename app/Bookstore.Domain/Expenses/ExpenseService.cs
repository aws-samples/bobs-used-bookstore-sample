namespace Bookstore.Domain.Expenses
{
    public interface IExpenseService
    {
        Task<Expense> GetBookstoreExpensesAsync();
    }

    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository expenseRepository;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            this.expenseRepository = expenseRepository;
        }

        public Task<Expense> GetBookstoreExpensesAsync()
        {
            throw new NotImplementedException();
        }
    }
}