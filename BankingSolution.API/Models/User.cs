namespace BankingSolution.API.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}
