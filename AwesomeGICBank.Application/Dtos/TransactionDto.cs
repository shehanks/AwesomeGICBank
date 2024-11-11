using AwesomeGICBank.Core.Enums;

namespace AwesomeGICBank.Application.Dtos
{
    public class TransactionDto
    {
        public DateTime Date { get; set; }

        public string TxnId { get; set; } = string.Empty;

        public TransactionType Type { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }
    }
}
