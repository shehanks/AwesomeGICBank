using AwesomeGICBank.Core.Contracts;
using AwesomeGICBank.Core.Entities;
using AwesomeGICBank.Infrastructure.DataAccess;

namespace AwesomeGICBank.Infrastructure.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(BankDbContext context)
            : base(context)
        {
        }
    }
}
