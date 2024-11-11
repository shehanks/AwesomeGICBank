using AwesomeGICBank.Core.Entities;

namespace AwesomeGICBank.Core.Contracts
{
    public interface ITransactionRepository : IRepositoryBase<Transaction>
    {
        // Add method definitions of repository specific methods if required.
        // Override base repository methods if required.

        /// <summary>
        /// This method is for fetching all the transactions up to enddate, to calculate running balance up to enddate.
        /// Additionally, using projection where only Type and Amount are fetched to optimize performance.
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<decimal> GetQueryableTransactionsUntilDateAsync(string accountNumber, DateTime endDate);
    }
}
