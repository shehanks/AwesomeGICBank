using AwesomeGICBank.Core.Contracts;
using AwesomeGICBank.Core.Entities;
using AwesomeGICBank.Core.Enums;
using AwesomeGICBank.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace AwesomeGICBank.Infrastructure.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(BankDbContext context)
            : base(context)
        {
        }

        public async Task<decimal> GetQueryableTransactionsUntilDateAsync(string accountNumber, DateTime endDate)
        {
            return await dbContext.Transactions
                .Where(t => t.BankAccount!.AccountNumber == accountNumber && t.Date <= endDate)
                .SumAsync(t => t.Type == TransactionType.D ? t.Amount : -t.Amount);
        }
    }
}
