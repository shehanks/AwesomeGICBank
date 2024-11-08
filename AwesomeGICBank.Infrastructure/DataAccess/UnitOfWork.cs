using AwesomeGICBank.Core.Contracts;
using AwesomeGICBank.Infrastructure.Repositories;

namespace AwesomeGICBank.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool disposed = false;
        private readonly BankDbContext _bankDbContext;
        private IBankAccountRepository? _bankAccountRepository;
        private ITransactionRepository? _transactionRepository;
        private IInterestRuleRepository? _interestRuleRepository;

        public UnitOfWork(BankDbContext dbContext)
        {
            _bankDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IBankAccountRepository BankAccountRepository =>
            _bankAccountRepository ?? (_bankAccountRepository = new BankAccountRepository(_bankDbContext));

        public IInterestRuleRepository InterestRuleRepository =>
            _interestRuleRepository ?? (_interestRuleRepository = new InterestRuleRepository(_bankDbContext));

        public ITransactionRepository TransactionRepository =>
            _transactionRepository ?? (_transactionRepository = new TransactionRepository(_bankDbContext));

        public int Complete() => _bankDbContext.SaveChanges();

        public async Task<int> CompleteAsync() => await _bankDbContext.SaveChangesAsync();

        protected virtual async ValueTask DisposeAsync(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    await _bankDbContext.DisposeAsync();
            }
            disposed = true;
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }
    }
}
