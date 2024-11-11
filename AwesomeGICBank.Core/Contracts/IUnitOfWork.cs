namespace AwesomeGICBank.Core.Contracts
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IBankAccountRepository BankAccountRepository { get; }

        IInterestRuleRepository InterestRuleRepository { get; }

        ITransactionRepository TransactionRepository { get; }

        int Complete();

        Task<int> CompleteAsync();
    }
}
