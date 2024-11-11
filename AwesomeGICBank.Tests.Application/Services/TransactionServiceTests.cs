using AutoMapper;
using AwesomeGICBank.Application.Contracts;
using AwesomeGICBank.Application.Dtos;
using AwesomeGICBank.Application.Services;
using AwesomeGICBank.Core.Contracts;
using AwesomeGICBank.Core.Entities;
using AwesomeGICBank.Core.Enums;
using Moq;
using System.Linq.Expressions;

namespace AwesomeGICBank.Tests.Application.Services
{
    public class TransactionServiceTests
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ITransactionService _transactionService;

        public TransactionServiceTests()
        {
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _transactionService = new TransactionService(_mapperMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task CreateTransactionAsync_ShouldReturnNull_WhenAccountDoesNotExistAndTypeIsWithdrawal()
        {
            // Arrange
            var createRequest = new CreateTransactionRequest
            {
                CreationDate = DateTime.Now,
                AccountNumber = "AC001",
                Amount = 100,
                Type = TransactionType.W
            };

            _unitOfWorkMock.Setup(u => u.BankAccountRepository.GetByAccountNumberAsync(createRequest.AccountNumber))
                .ReturnsAsync(null as BankAccount);

            // Act
            var result = await _transactionService.CreateTransactionAsync(createRequest);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateTransactionAsync_ShouldCreateNewAccount_WhenAccountDoesNotExistAndTypeIsDeposit()
        {
            // Arrange
            var transactionDate = new DateTime(2023, 10, 25);

            var createRequest = new CreateTransactionRequest
            {
                AccountNumber = "AC001",
                Amount = 1000.55m,
                Type = TransactionType.D
            };

            var transaction = new Transaction
            {
                Id = 1,
                TxnId = 1,
                Amount = 1000.55m,
                Type = TransactionType.D,
                Date = transactionDate,
                BankAccountId = 1,
            };

            var transactionDto = new TransactionDto
            {
                Date = transactionDate,
                Amount = 1000.55m,
                Type = TransactionType.D,
                Balance = 1000.55m,
                TxnId = "20231025-1"
            };

            _unitOfWorkMock.Setup(u => u.BankAccountRepository.GetByAccountNumberAsync(createRequest.AccountNumber))
                .ReturnsAsync(null as BankAccount);
            _mapperMock.Setup(m => m.Map<Transaction>(createRequest)).Returns(transaction);
            _unitOfWorkMock.Setup(u => u.TransactionRepository.InsertAsync(transaction)).ReturnsAsync(transaction);
            _mapperMock.Setup(m => m.Map<TransactionDto>(transaction)).Returns(transactionDto);

            // Act
            var result = await _transactionService.CreateTransactionAsync(createRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(transactionDto, result);

            _unitOfWorkMock.Verify(u => u.TransactionRepository.InsertAsync(transaction), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTransactionsByAccountAsync_ShouldReturnMappedTransactions_WhenAccountExists()
        {
            // Arrange
            var accountNumber = "AC001";
            var transactionDate1 = new DateTime(2023, 10, 25);
            var transactionDate2 = new DateTime(2023, 10, 27);

            var account = new BankAccount { Id = 1, AccountNumber = accountNumber };
            var transactions = new List<Transaction>
            {
                new Transaction { BankAccountId = account.Id, TxnId = 1, Date = transactionDate1 },
                new Transaction { BankAccountId = account.Id, TxnId = 2, Date = transactionDate2 },
            };

            var transactionDtos = new List<TransactionDto>
            {
                new TransactionDto { Date = transactionDate1, TxnId ="20231025-1" },
                new TransactionDto { Date = transactionDate2, TxnId = "20231027-2" }
            };

            _unitOfWorkMock.Setup(u => u.BankAccountRepository.GetByAccountNumberAsync(accountNumber))
                .ReturnsAsync(account);

            _unitOfWorkMock.Setup(u => u.TransactionRepository.GetAsync(
                It.IsAny<Expression<Func<Transaction, bool>>>(),
                It.IsAny<Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>>>(),
                null, null, string.Empty))
                .ReturnsAsync(transactions);
            _mapperMock.Setup(m => m.Map<List<TransactionDto>>(transactions)).Returns(transactionDtos);

            // Act
            var result = await _transactionService.GetTransactionsByAccountAsync(accountNumber);

            // Assert
            Assert.Equal(transactionDtos, result);

            _mapperMock.Verify(m => m.Map<List<TransactionDto>>(transactions), Times.Once);
        }

        [Fact]
        public async Task GetTransactionsForMonthAsync_ShouldReturnMappedTransactions_WhenAccountExists()
        {
            // Arrange
            var accountNumber = "AC001";
            var year = 2024;
            var month = 11;
            var account = new BankAccount { Id = 1, AccountNumber = accountNumber };

            var transactions = new List<Transaction>
            {
                new Transaction { BankAccountId = account.Id, Date = new DateTime(year, month, 1) },
                new Transaction { BankAccountId = account.Id, Date = new DateTime(year, month, 2) }
            };

            var transactionDtos = new List<TransactionDto>
            {
                new TransactionDto { TxnId ="20241101-1" },
                new TransactionDto { TxnId = "20241102-2" }
            };

            _unitOfWorkMock.Setup(u => u.BankAccountRepository.GetByAccountNumberAsync(accountNumber))
                .ReturnsAsync(account);
            _unitOfWorkMock.Setup(u => u.TransactionRepository.GetAsync(
                It.IsAny<Expression<Func<Transaction, bool>>>(),
                It.IsAny<Func<IQueryable<Transaction>, IOrderedQueryable<Transaction>>>(),
                null, null, string.Empty))
                .ReturnsAsync(transactions);
            _mapperMock.Setup(m => m.Map<List<TransactionDto>>(transactions)).Returns(transactionDtos);

            // Act
            var result = await _transactionService.GetTransactionsForMonthAsync(accountNumber, year, month);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(transactionDtos, result);

            _mapperMock.Verify(m => m.Map<List<TransactionDto>>(transactions), Times.Once);
        }

        [Fact]
        public async Task GetTransactionsUntilDateAsync_ShouldReturnCorrectBalance()
        {
            // Arrange
            var accountNumber = "AC001";
            var endDate = DateTime.Today;
            decimal expectedBalance = 1000.56m;

            _unitOfWorkMock.Setup(u => u.TransactionRepository.GetQueryableTransactionsUntilDateAsync(accountNumber, endDate))
                .ReturnsAsync(expectedBalance);

            // Act
            var result = await _transactionService.GetTransactionsUntilDateAsync(accountNumber, endDate);

            // Assert
            Assert.Equal(expectedBalance, result);

            _unitOfWorkMock.Verify(u => u.TransactionRepository.GetQueryableTransactionsUntilDateAsync(accountNumber, endDate), Times.Once);
        }
    }
}
