using AwesomeGICBank.Application.Contracts;
using AwesomeGICBank.Core.Contracts;
using AwesomeGICBank.Core.Entities;

namespace AwesomeGICBank.Application.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BankAccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAccountAsync(BankAccount account)
        {
            _unitOfWork.BankAccountRepository.Insert(account);
            await _unitOfWork.CompleteAsync();
        }
    }
}
