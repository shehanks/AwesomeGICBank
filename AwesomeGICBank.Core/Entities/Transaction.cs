using AwesomeGICBank.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace AwesomeGICBank.Core.Entities
{
    public class Transaction : EntityBase
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int TxnId { get; set; }

        // Foreign Key
        public int BankAccountId { get; set; }

        public BankAccount? BankAccount { get; set; }
    }
}
