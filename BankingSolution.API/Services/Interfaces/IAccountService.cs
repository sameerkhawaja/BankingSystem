using BankingSolution.API.Models;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace BankingSolution.API.Services.Interfaces
{
    public interface IAccountService
    {
        public Account CreateAccount(Guid userId, decimal deposit);
        public decimal GetBalance(Guid userId, string accountNumber);
        public Account Deposit(Guid userId, string accountNumber, decimal amount);
        public Account Withdraw(Guid userId, string accountNumber, decimal amount);
        public bool DeleteAccount(Guid userId, string accountNumber);
    }
}
