using BankingSolution.API.Models;
using BankingSolution.API.Services.Interfaces;
using Microsoft.AspNetCore.Server.IIS.Core;
using System.ComponentModel;
using System.Linq;

namespace BankingSolution.API.Services
{
    //dummy database
    public class AccountService : IAccountService
    {
        private IMockRepository _mockDatabase;

        public AccountService(IMockRepository mockDatabase) {
            _mockDatabase = mockDatabase;
        }

        public Account GetAccount(Guid userId, string accountNumber)
        {
            return _mockDatabase.GetAccount(userId, accountNumber);
        }

        public Account CreateAccount(Guid userId, decimal deposit)
        {
            if (deposit < 100)
            {
                throw new ArgumentException($"Initial deposit must be greater than $100.00. UserId: '{userId}' Initial deposit: '${deposit}'");
            }
            else if (deposit > 10_000)
            {
                throw new ArgumentException($"Initial deposit cannot be greater than $10,000.00. UserId: '{userId}' Initial deposit: '${deposit}'");
            }

            var account = new Account()
            {
                UserId = userId,
                Balance = deposit
            };

            var success = _mockDatabase.AddAccount(account);

            if (success)
            {
                return account;
            }
            else
            {
                throw new Exception($"Create account failed for userId:'{userId}'");
            }
        }

        public bool DeleteAccount(Guid userId, string accountNumber)
        {
            var response = _mockDatabase.DeleteAccount(userId,accountNumber);
            return response;
        }

        public Account Deposit(Guid userId, string accountNumber, decimal amount)
        {
            if (amount > 10000)
            {
                throw new ArgumentException("Cannot deposit more than $10,000 in a single transaction.");
            }

            return _mockDatabase.UpdateBalance(userId, accountNumber, amount);
        }

        public Account Withdraw(Guid userId, string accountNumber, decimal amount)
        {
            var balance = GetBalance(userId, accountNumber);

            if (amount <= 0)
            {
                throw new ArgumentException($"Withdraw amount should be greater than $0. AccountNumber: '{accountNumber}'");
            }

            if (amount > 0.9m * balance)
            {
                throw new ArgumentException("Cannot withdraw more than 90% of the total balance in a single transaction.");
            }

            if (balance - amount < 100)
            {
                throw new ArgumentException("Account cannot have less than $100 at any time.");
            }

            return _mockDatabase.UpdateBalance(userId, accountNumber, -amount);
        }

        public decimal GetBalance(Guid userId, string accountNumber)
        {
            return _mockDatabase.GetBalance(userId, accountNumber);
        }
    }
}
