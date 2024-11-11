namespace AwesomeGICBank.Application.Contracts
{
    public interface IBankingServiceCoordinator
    {
        Task RunAsync();

        Task ProcessTransactionAsync();

        Task DefineInterestRuleAsync();

        Task PrintStatementAsync();
    }
}
