using BankingSolution.API.Models;
using BankingSolution.API.Services.Interfaces;

namespace BankingSolution.API.Services
{
    public class UserService: IUserService
    {
        //dummy get user
        public User GetUser(string name)
        {
            return new User
            {
                Name = name
            };
        }

        //dummy get user
        public User GetUser(Guid id)
        {
            return new User
            {
                Id = id,
                Name = "Pepe Silvia"
            };
        }
    }
}
