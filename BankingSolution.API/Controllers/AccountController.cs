using BankingSolution.API.Models;
using BankingSolution.API.Services;
using BankingSolution.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;

namespace BankingSolution.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet(Name = "GetAccountBalance")]
        public IActionResult GetBalance(Guid userId, string accountNumber)
        {
            try
            {
                var accountBalance = _accountService.GetBalance(userId, accountNumber);
                return Ok(accountBalance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(Name = "CreateAccount")]
        public ActionResult<Account> CreateAccount(Guid userId, decimal deposit)
        {
            try
            {
                var account = _accountService.CreateAccount(userId, deposit);
                return CreatedAtAction(nameof(CreateAccount), account);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{accountNumber}")]
        public IActionResult DeleteAccount(Guid userId, string accountNumber)
        {
            try
            {
                _accountService.DeleteAccount(userId, accountNumber);
                return Ok($"Account '{accountNumber}' deleted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("deposit/{accountNumber}")]
        public IActionResult Deposit(Guid userId, string accountNumber, decimal amount)
        {
            try
            {
                var account = _accountService.Deposit(userId, accountNumber, amount);
                return Ok($"Deposit successful. Account '{account.AccountNumber}' has balance of '${account.Balance}'");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("withdraw/{accountNumber}")]
        public IActionResult Withdraw(Guid userId, string accountNumber, decimal amount)
        {
            try
            {
                var account = _accountService.Withdraw(userId, accountNumber, amount);
                return Ok($"Withdrawal successful. Account '{account.AccountNumber}' has balance of '${account.Balance}'");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
