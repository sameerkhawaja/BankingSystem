using BankingSolution.API.Models;

namespace BankingSolution.API.Services.Interfaces
{
    public interface IUserService
    {
        public User GetUser(string name);

        public User GetUser(Guid id);
    }
}
