using AwesomeGICBank.Core.Entities;

namespace AwesomeGICBank.Application.Contracts
{
    public interface IBankAccountService
    {
        Task CreateAccountAsync(BankAccount account);
    }
}
