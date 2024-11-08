using AwesomeGICBank.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace AwesomeGICBank.Core.Entities
{
    public class BankAccount : EntityBase
    {
        public BankAccount() { }

        public BankAccount(string accountNumber, decimal initialAmount)
        {
            AccountNumber = accountNumber;
            Balance = initialAmount;
            Transactions.Add(new Transaction
            {
                Type = TransactionType.D,
                Amount = initialAmount,
                Date = DateTime.UtcNow
            });
        }

        [Required]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        public decimal Balance { get; set; }

        // Navigation property
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
