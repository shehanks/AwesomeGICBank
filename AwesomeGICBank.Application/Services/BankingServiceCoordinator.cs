﻿using AwesomeGICBank.Application.Contracts;
using AwesomeGICBank.Application.Dtos;
using AwesomeGICBank.Application.Helpers;
using AwesomeGICBank.Core.Enums;
using System.Collections.Immutable;
using System.Globalization;

namespace AwesomeGICBank.Application.Services
{
    public class BankingServiceCoordinator : IBankingServiceCoordinator
    {
        private readonly ITransactionService _transactionService;
        private readonly IInterestService _interestService;

        public BankingServiceCoordinator(
            ITransactionService transactionService,
            IInterestService interestService)
        {
            _transactionService = transactionService;
            _interestService = interestService;
        }

        public async Task Run()
        {
            bool exit = false;
            bool firstTime = true;

            while (!exit)
            {
                if (firstTime)
                {
                    Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
                    firstTime = false;
                }
                else
                {
                    Console.WriteLine("Is there anything else you'd like to do?");
                }

                ShowMainMenu();

                var mainSelection = Console.ReadLine()?.ToUpper();
                switch (mainSelection)
                {
                    case "T":
                        await ProcessTransactionAsync();
                        break;
                    case "I":
                        await DefineInterestRuleAsync();
                        break;
                    case "P":
                        await PrintStatementAsync();
                        break;
                    case "Q":
                        exit = true;
                        Console.WriteLine("Thank you for banking with AwesomeGIC Bank.\nHave a nice day!");
                        return;
                    default:
                        break;
                }
            }
        }

        #region Main Menu Methods 

        async Task ProcessTransactionAsync()
        {
            while (true)
            {
                Console.WriteLine("Please enter transaction details in <Date> <Account> <Type> <Amount> format \n(or enter blank to go back to main menu):");
                Console.Write("> ");

                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) break;

                var parameters = input.Split(' ');

                if (parameters.Length != 4 ||
                    !DateTime.TryParseExact(parameters[0], "yyyyMMdd", null, DateTimeStyles.None, out DateTime transactionDate) ||
                    string.IsNullOrWhiteSpace(parameters[1]) ||
                    !decimal.TryParse(parameters[3], out decimal transactionAmount) || transactionAmount <= 0)
                {
                    Console.WriteLine("Invalid input format. Please try again.");
                    continue;
                }

                var accountNumber = parameters[1];

                if (!EnumHelper.TryParseEnum(parameters[2].ToUpper(), out TransactionType transactionType))
                {
                    Console.WriteLine("Invalid transaction type. Use 'D' for deposit and 'W' for withdrawal.");
                    continue;
                }

                var transactionRequest = new CreateTransactionRequest
                {
                    AccountNumber = accountNumber,
                    Type = transactionType,
                    Amount = transactionAmount,
                    CreationDate = transactionDate
                };

                var transaction = await _transactionService.CreateTransactionAsync(transactionRequest);

                if (transaction is null)
                {
                    Console.WriteLine("Insufficient funds or invalid transaction. Balance cannot go below zero.");
                    continue;
                }

                await PrintTransactions(accountNumber);

                Console.WriteLine();
                break;
            }
        }

        async Task DefineInterestRuleAsync()
        {
            while (true)
            {
                Console.WriteLine("Please enter interest rules details in <Date> <RuleId> <Rate in %> format \n(or enter blank to go back to main menu):");
                Console.Write("> ");

                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) break;

                var parameters = input.Split(' ');

                if (parameters.Length != 3 ||
                    !DateTime.TryParseExact(parameters[0], "yyyyMMdd", null, DateTimeStyles.None, out DateTime ruleDate) ||
                    string.IsNullOrWhiteSpace(parameters[1]) ||
                    !decimal.TryParse(parameters[2], out decimal ratePercentage) || (ratePercentage is <= 0 or >= 100))
                {
                    Console.WriteLine("Invalid input format. Please try again.");
                    continue;
                }

                var ruleId = parameters[1];

                var transactionRequest = new CreateInterestRuleRequest
                {
                    RuleId = ruleId,
                    RatePercentage = ratePercentage,
                    CreationDate = ruleDate
                };

                var rule = await _interestService.CreateInterestRuleAsync(transactionRequest);

                await PrintRules();

                Console.WriteLine();
                break;
            }
        }

        async Task PrintStatementAsync()
        {
            while (true)
            {
                Console.WriteLine("Please enter account and month to generate the statement <Account> <Year><Month> \n(or enter blank to go back to main menu):");
                Console.Write("> ");

                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) break;

                var parameters = input.Split(' ');

                if (parameters.Length != 2 ||
                    string.IsNullOrWhiteSpace(parameters[0]) ||
                    !DateTime.TryParseExact($"{parameters[1]}01", "yyyyMMdd", null, DateTimeStyles.None, out DateTime statementDate))
                {
                    Console.WriteLine("Invalid input format. Please try again.");
                    continue;
                }

                var accountNumber = parameters[0];
                var monthStatements = await GetMonthlyStatementAsync(accountNumber, statementDate);

                if (monthStatements?.Any() == false)
                {
                    Console.WriteLine("Account number does not exist or no statement records for the selected month.");
                    break;
                }

                var totalInterest = await GetMonthlyInterest(statementDate, monthStatements!);

                monthStatements!.Add(new TransactionDto
                {
                    Date = new DateTime(statementDate.Year, statementDate.Month, DateTime.DaysInMonth(statementDate.Year, statementDate.Month)),
                    Type = TransactionType.I,
                    Amount = totalInterest,
                    Balance = monthStatements.Last().Balance + totalInterest,
                });

                PrintAccountStatement(accountNumber, monthStatements);

                Console.WriteLine();
                break;
            }
        }

        #endregion

        #region Support Methods

        async Task<decimal> GetMonthlyInterest(DateTime statementDate, List<TransactionDto> monthStatements)
        {
            decimal totalInterest = 0;
            var rules = await _interestService.GetAllRules();

            var lastDayOfMOnth = DateTime.DaysInMonth(statementDate.Year, statementDate.Month);
            var statementDateMargins = ImmutableList<(DateTime startDate, DateTime endDate, int numberOfDays)>.Empty
                .Add((new DateTime(statementDate.Year, statementDate.Month, 1), new DateTime(statementDate.Year, statementDate.Month, 14), 14))
                .Add((new DateTime(statementDate.Year, statementDate.Month, 15), new DateTime(statementDate.Year, statementDate.Month, 25), 11))
                .Add((new DateTime(statementDate.Year, statementDate.Month, 26), new DateTime(statementDate.Year, statementDate.Month, lastDayOfMOnth), lastDayOfMOnth - 25));

            if (rules?.Any() == true)
            {
                foreach (var (startDate, endDate, numberOfDays) in statementDateMargins)
                {
                    var nearestRule = rules
                        .Where(rule => rule.Date <= endDate)
                        .OrderByDescending(rule => rule.Date)
                        .ThenByDescending(rule => rule.Id)
                        .First();

                    var nearestTransaction = monthStatements!.Where(transaction => transaction.Date <= endDate)
                        .OrderByDescending(transaction => transaction.Date)
                        .ThenByDescending(transaction => transaction.TxnId)
                        .First();

                    totalInterest += (nearestTransaction.Balance * nearestRule.InterestRate * numberOfDays) / 100;
                }

                totalInterest = Math.Round(totalInterest / 365, 2);
            }

            return totalInterest;
        }

        async Task<List<TransactionDto>?> GetMonthlyStatementAsync(string accountNumber, DateTime statementDate)
        {
            var startingBalance = await _transactionService.GetTransactionsUntilDate(accountNumber, statementDate);

            // Fetch all transactions for the current month
            var transactions = await _transactionService.GetTransactionsForMonth(accountNumber, statementDate.Year, statementDate.Month);

            // Start with the calculated starting balance
            decimal runningBalance = startingBalance;

            if (transactions?.Any() == true)
            {
                transactions.First().Balance = runningBalance;

                if (transactions.Count > 1)
                {
                    for (int i = 1; i < transactions.Count; i++)
                    {
                        var transaction = transactions[i];
                        if (transaction.Type == TransactionType.W)
                        {
                            runningBalance -= transaction.Amount;
                            transaction.Balance = runningBalance;
                        }
                    }
                }
            }

            return transactions;
        }

        #endregion

        #region Print Menus and Results

        static void ShowMainMenu()
        {
            Console.WriteLine("[T] Input transactions");
            Console.WriteLine("[I] Define interest rules");
            Console.WriteLine("[P] Print statement");
            Console.WriteLine("[Q] Quit");
            Console.Write("> ");
        }

        async Task PrintTransactions(string accountNumber)
        {
            var transactions = await _transactionService.GetTransactionsByAccount(accountNumber);

            if (transactions?.Any() == true)
            {
                Console.WriteLine($"Account: {accountNumber}");
                Console.WriteLine("| Date     | Txn Id      | Type | Amount |");
                foreach (var transaction in transactions)
                {
                    Console.WriteLine($"| {transaction.Date:yyyyMMdd} | {transaction.TxnId} | {transaction.Type}    | {transaction.Amount,7:F2}");
                }
            }
        }

        async Task PrintRules()
        {
            var interestRules = await _interestService.GetAllRules();

            if (interestRules?.Any() == true)
            {
                Console.WriteLine("Interest rules:");
                Console.WriteLine("| Date     | RuleId | Rate (%) |");
                foreach (var rule in interestRules)
                {
                    Console.WriteLine($"| {rule.Date:yyyyMMdd} | {rule.RuleId} | {rule.InterestRate,8:F2} |");
                }
            }
        }

        void PrintAccountStatement(string accountNumber, List<TransactionDto> transactions)
        {
            if (transactions?.Any() == true)
            {
                Console.WriteLine($"Account: {accountNumber}");
                Console.WriteLine("| Date     | Txn Id      | Type | Amount   | Balance  |");
                Console.WriteLine("|----------|-------------|------|----------|----------|");
                foreach (var transaction in transactions)
                {
                    Console.WriteLine(
                        $"| {transaction.Date,-8:yyyyMMdd} | {transaction.TxnId,-11} | {transaction.Type,-4} | {transaction.Amount,8:F2} | {transaction.Balance,8:F2} |");
                }
            }
        }

        #endregion
    }
}
