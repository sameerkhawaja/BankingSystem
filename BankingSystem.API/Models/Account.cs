namespace BankingSolution.API.Models
{
    public class Account
    {
        public string AccountNumber { get; set; } = GenerateAccountNumber();

        public decimal Balance { get; set; }

        public Guid UserId{ get; set; }

        private static string GenerateAccountNumber()
        {
            Random generator = new();
            string r = generator.Next(0, 1000000).ToString("D6");
            return r;
        }
    }


}
