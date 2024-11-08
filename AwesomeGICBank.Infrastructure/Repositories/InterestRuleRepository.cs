using AwesomeGICBank.Core.Contracts;
using AwesomeGICBank.Core.Entities;
using AwesomeGICBank.Infrastructure.DataAccess;

namespace AwesomeGICBank.Infrastructure.Repositories
{
    public class InterestRuleRepository : RepositoryBase<InterestRule>, IInterestRuleRepository
    {
        public InterestRuleRepository(BankDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
