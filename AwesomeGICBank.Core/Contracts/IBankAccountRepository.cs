using AwesomeGICBank.Core.Entities;

namespace AwesomeGICBank.Core.Contracts
{
    public interface IBankAccountRepository : IRepositoryBase<BankAccount>
    {
        // Add method definitions of repository specific methods if required.
        // Override base repository methods if required.

        Task<BankAccount?> GetByAccountNumberAsync(string accountNumber);
    }
}
