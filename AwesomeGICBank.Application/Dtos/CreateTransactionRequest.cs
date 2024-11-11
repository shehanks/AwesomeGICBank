using AwesomeGICBank.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace AwesomeGICBank.Application.Dtos
{
    public class CreateTransactionRequest : RequestBase
    {
        [Required]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        public TransactionType Type { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
