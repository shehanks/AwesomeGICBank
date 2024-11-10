using AwesomeGICBank.Core.Contracts;
using AwesomeGICBank.Core.Entities;
using AwesomeGICBank.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace AwesomeGICBank.Infrastructure.Repositories
{
    public class BankAccountRepository : RepositoryBase<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(BankDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<BankAccount?> GetByAccountNumber(string accountNumber)
        {
            return await dbContext.BankAccounts.FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);
        }
    }
}
