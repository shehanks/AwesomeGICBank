using AwesomeGICBank.Core.Contracts;
using AwesomeGICBank.Core.Entities;
using AwesomeGICBank.Infrastructure.DataAccess;

namespace AwesomeGICBank.Infrastructure.Repositories
{
    public class BankAccountRepository : RepositoryBase<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(BankDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
