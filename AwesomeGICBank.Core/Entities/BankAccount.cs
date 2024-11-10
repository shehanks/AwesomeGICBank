using System.ComponentModel.DataAnnotations;

namespace AwesomeGICBank.Core.Entities
{
    public class BankAccount : EntityBase
    {
        [Required]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        public decimal Balance { get; set; }

        // Navigation property
        public List<Transaction>? Transactions { get; set; }
    }
}
