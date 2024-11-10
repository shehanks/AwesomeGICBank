using AutoMapper;
using AwesomeGICBank.Application.Contracts;
using AwesomeGICBank.Application.Dtos;
using AwesomeGICBank.Core.Contracts;
using AwesomeGICBank.Core.Entities;
using AwesomeGICBank.Core.Enums;

namespace AwesomeGICBank.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionDto?> CreateTransactionAsync(CreateTransactionRequest createTransactionRequest)
        {
            var account = await _unitOfWork.BankAccountRepository.GetByAccountNumber(createTransactionRequest.AccountNumber);
            var transaction = _mapper.Map<Transaction>(createTransactionRequest);

            if (account is null)
            {
                if (createTransactionRequest.Type == TransactionType.W)
                    return null;

                transaction.BankAccount = new BankAccount()
                {
                    AccountNumber = createTransactionRequest.AccountNumber,
                    Balance = createTransactionRequest.Amount
                };
                transaction.TxnId = 1;
                await _unitOfWork.TransactionRepository.InsertAsync(transaction);
            }
            else
            {
                if (createTransactionRequest.Type == TransactionType.W &&
                    createTransactionRequest.Amount > account.Balance)
                {
                    return null;
                }

                var nearestTransactions = await _unitOfWork.TransactionRepository.GetAsync(
                    filter: transaction =>
                        transaction.Date.Month == createTransactionRequest.CreationDate.Month &&
                        transaction.Date.Day == createTransactionRequest.CreationDate.Day,
                    orderBy: query =>
                        query.OrderByDescending(e => e.Id));

                var lastTransaction = nearestTransactions.FirstOrDefault();

                transaction.TxnId = lastTransaction is null ? 1 : lastTransaction.TxnId + 1;
                transaction.BankAccountId = account.Id;
                await _unitOfWork.TransactionRepository.InsertAsync(transaction);

                account.Balance = (createTransactionRequest.Type == TransactionType.W) ?
                    account.Balance -= createTransactionRequest.Amount : account.Balance += createTransactionRequest.Amount;
            }

            await _unitOfWork.CompleteAsync();
            return _mapper.Map<TransactionDto>(transaction);
        }

        public async Task<List<TransactionDto>> GetTransactionsByAccount(string accountNumber)
        {
            var account = await _unitOfWork.BankAccountRepository.GetByAccountNumber(accountNumber);

            if (account is not null)
            {
                var transactions = await _unitOfWork.TransactionRepository.GetAsync(
                    filter: x => x.BankAccountId == account.Id,
                    orderBy: query => query.OrderBy(e => e.Date));

                return _mapper.Map<List<TransactionDto>>(transactions);
            }

            return await Task.FromResult(new List<TransactionDto>());
        }

        public async Task<List<TransactionDto>> GetTransactionsForMonth(string accountNumber, int year, int month)
        {
            var account = await _unitOfWork.BankAccountRepository.GetByAccountNumber(accountNumber);

            if (account is not null)
            {
                var transactions = await _unitOfWork.TransactionRepository.GetAsync(
                    filter: x => x.BankAccountId == account.Id && x.Date.Year == year && x.Date.Month == month,
                    orderBy: query => query.OrderBy(e => e.Date));

                return _mapper.Map<List<TransactionDto>>(transactions);
            }

            return await Task.FromResult(new List<TransactionDto>());
        }

        public async Task<decimal> GetTransactionsUntilDate(string accountNumber, DateTime endDate) =>
            await _unitOfWork.TransactionRepository.GetQueryableTransactionsUntilDate(accountNumber, endDate);
    }
}
