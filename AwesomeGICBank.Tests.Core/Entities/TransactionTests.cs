using AwesomeGICBank.Core.Entities;
using AwesomeGICBank.Core.Enums;

namespace AwesomeGICBank.Tests.Core
{
    public class TransactionTests
    {
        [Fact]
        public void Create_Initial_Transaction_ShouldSetProperties()
        {
            // Arrange
            var transactionDate = new DateTime(2023, 10, 25);

            // Act
            var transaction = new Transaction()
            {
                Date = transactionDate,
                Type = TransactionType.D,
                Amount = 1000.23m,
                TxnId = 1,
                BankAccount = new BankAccount
                {
                    AccountNumber = "AC001",
                    Balance = 1000.23m
                }
            };

            // Assert
            Assert.Equal(TransactionType.D, transaction.Type);
            Assert.Equal(transactionDate, transaction.Date);
            Assert.Equal(1000.23m, transaction.Amount);
            Assert.Equal(1, transaction.TxnId);
            Assert.NotNull(transaction.BankAccount);
            Assert.Equal("AC001", transaction.BankAccount.AccountNumber);
            Assert.Equal(transaction.Amount, transaction.BankAccount.Balance);
        }

        [Theory]
        [InlineData(TransactionType.D, 1000, 1)]
        [InlineData(TransactionType.W, 2000, 2)]
        public void Create_Transaction_ForExistingAccount_ShouldSetProperties(
            TransactionType transactionType, decimal amount, int txnId)
        {
            // Act
            var transactionDate = new DateTime(2023, 10, 25);
            var transaction = new Transaction()
            {
                Date = transactionDate,
                Type = transactionType,
                Amount = amount,
                TxnId = txnId,
                BankAccountId = 1
            };

            // Assert
            Assert.True(transaction.BankAccountId > 0);
            Assert.Equal(transactionType, transaction.Type);
            Assert.Equal(transactionDate, transaction.Date);
            Assert.Equal(amount, transaction.Amount);
            Assert.True(transaction.TxnId > 0);
            Assert.Equal(txnId, transaction.TxnId);
            Assert.Null(transaction.BankAccount);
        }
    }
}