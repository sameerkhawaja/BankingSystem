using BankingSolution.API.Models;

namespace BankingSolution.API.Services.Interfaces
{
    public interface IMockRepository
    {
        public Account GetAccount(Guid userId, string accountNumber);

        public bool AddAccount(Account account);

        public bool DeleteAccount(Guid userId, string accountNumber);

        public Account UpdateBalance(Guid userId, string accountNumber, decimal amount);

        public decimal GetBalance(Guid userId, string accountNumber);

    }
}
