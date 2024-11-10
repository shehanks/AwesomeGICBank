using AwesomeGICBank.Application.Dtos;

namespace AwesomeGICBank.Application.Contracts
{
    public interface ITransactionService
    {
        Task<TransactionDto?> CreateTransactionAsync(CreateTransactionRequest createTransactionRequest);

        Task<List<TransactionDto>> GetTransactionsByAccount(string accountNumber);

        Task<List<TransactionDto>> GetTransactionsForMonth(string accountNumber, int year, int month);

        /// <summary>
        /// This method is for fetching all the transactions up to enddate, to calculate running balance up to enddate.
        /// Additionally, using projection where only Type and Amount are fetched to optimize performance.
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<decimal> GetTransactionsUntilDate(string accountNumber, DateTime endDate);
    }
}
