using BankingSolution.API.Models;
using BankingSolution.API.Services.Interfaces;

namespace BankingSolution.API.Services
{
    public class MockRepository : IMockRepository
    {
        private List<Account> _accounts = new List<Account>();

        public Account GetAccount(Guid userId, string accountNumber)
        {
            return _accounts.SingleOrDefault(x => x.AccountNumber == accountNumber && x.UserId == userId);
        }

        public bool AddAccount(Account account)
        {
            _accounts.Add(account);
            return true;
        }

        public bool DeleteAccount(Guid userId, string accountNumber)
        {
            var account = _accounts.SingleOrDefault(x => x.AccountNumber == accountNumber && x.UserId == userId);
            if (account != null)
            {
                _accounts.Remove(account);
                return true;
            }
            return false;

        }

        public Account UpdateBalance(Guid userId, string accountNumber, decimal amount)
        {
            var account = _accounts.SingleOrDefault(x => x.AccountNumber == accountNumber && x.UserId == userId);
            if (account != null)
            {
                account.Balance += amount;
                return account;
            };

            throw new Exception("Account not found");
        }

        public decimal GetBalance(Guid userId, string accountNumber)
        {
            return _accounts.SingleOrDefault(x => x.AccountNumber == accountNumber && x.UserId == userId).Balance;
        }
    }
}
