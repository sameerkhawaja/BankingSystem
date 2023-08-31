using BankingSolution.API.Models;
using BankingSolution.API.Services;
using BankingSolution.API.Services.Interfaces;
using Moq;
using System;

namespace BankingSolutionTests
{
    public class AccountServiceTests
    {
        private Mock<IMockRepository> _mockRepository;
        private AccountService _accountService;
        private Guid _testUserId;
        private string _testAccountNumber;

        public AccountServiceTests()
        {
            _mockRepository = new Mock<IMockRepository>();
            _accountService = new AccountService(_mockRepository.Object);
            _testUserId = Guid.NewGuid();
            _testAccountNumber = "123456";
        }

        [Fact]
        public void GetAccount_GetsAccountFromDatabase()
        {
            // Arrange
            var expectedAccount = new Account 
            { 
                UserId = _testUserId,
                AccountNumber = _testAccountNumber, 
                Balance = 10000m 
            };

            _mockRepository.Setup(db => db.GetAccount(_testUserId, _testAccountNumber)).Returns(expectedAccount);

            // Act
            var result = _accountService.GetAccount(_testUserId, _testAccountNumber);

            // Assert
            Assert.Equal(expectedAccount, result);
        }

        [Fact]
        public void CreateAccount_WhenDatabaseFails_ThrowsException()
        {
            // Arrange
            var depositAmount = 500m;
            _mockRepository.Setup(db => db.AddAccount(It.IsAny<Account>())).Returns(false);

            // Act
            var exception = Assert.Throws<Exception>(() => _accountService.CreateAccount(_testUserId, depositAmount));

            // Assert
            Assert.Contains($"Create account failed for userId:'{_testUserId}'", exception.Message);
        }

        [Fact]
        public void CreateAccount_WhenDepositIsLessThan100_ThrowsArgumentException()
        {
            // Arrange
            var depositAmount = 99.99m;

            // Act
            var exception = Assert.Throws<ArgumentException>(() => _accountService.CreateAccount(_testUserId, depositAmount));

            // Assert
            Assert.Contains($"Initial deposit must be greater than $100.00. UserId: '{_testUserId}' Initial deposit: '${depositAmount}'", exception.Message);
        }

        [Fact]
        public void CreateAccount_WhenDepositIs100_CreatesAccount()
        {
            // Arrange
            var depositAmount = 100m;
            _mockRepository.Setup(db => db.AddAccount(It.IsAny<Account>())).Returns(true);

            // Act
            var result = _accountService.CreateAccount(_testUserId, depositAmount);

            // Assert
            _mockRepository.Verify(db => db.AddAccount(It.Is<Account>(a => a.UserId == _testUserId && a.Balance == depositAmount)));
            Assert.Equal(depositAmount, result.Balance);
            Assert.Equal(_testUserId, result.UserId);
        }

        [Fact]
        public void CreateAccount_WhenDepositIs10000_CreatesAccount()
        {
            // Arrange
            var depositAmount = 10_000m;
            _mockRepository.Setup(db => db.AddAccount(It.IsAny<Account>())).Returns(true);

            // Act
            var result = _accountService.CreateAccount(_testUserId, depositAmount);

            // Assert
            _mockRepository.Verify(db => db.AddAccount(It.Is<Account>(a => a.UserId == _testUserId && a.Balance == depositAmount)));
            Assert.Equal(depositAmount, result.Balance);
            Assert.Equal(_testUserId, result.UserId);
        }

        [Fact]
        public void CreateAccount_WhenDepositIsGreaterThan10000_ThrowsArgumentException()
        {
            // Arrange
            var depositAmount = 10_000.01m;

            // Act
            var exception = Assert.Throws<ArgumentException>(() => _accountService.CreateAccount(_testUserId, depositAmount));
            
            // Assert
            Assert.Contains($"Initial deposit cannot be greater than $10,000.00. UserId: '{_testUserId}' Initial deposit: '${depositAmount}'", exception.Message);
        }

        [Fact]
        public void CreateAccount_CreatesAccount()
        {
            // Arrange
            var depositAmount = 10000m;
            _mockRepository.Setup(db => db.AddAccount(It.IsAny<Account>())).Returns(true);

            // Act
            var result = _accountService.CreateAccount(_testUserId, depositAmount);

            // Assert
            _mockRepository.Verify(db => db.AddAccount(It.Is<Account>(a => a.UserId == _testUserId && a.Balance == depositAmount)));
            Assert.Equal(depositAmount, result.Balance);
            Assert.Equal(_testUserId, result.UserId);
        }

        [Fact]
        public void DeleteAccount_RemovesAccountFromDatabase()
        {
            // Arrange
            _mockRepository.Setup(db => db.DeleteAccount(_testUserId, _testAccountNumber)).Returns(true);

            // Act
            var result = _accountService.DeleteAccount(_testUserId, _testAccountNumber);

            // Assert
            _mockRepository.Verify(db => db.DeleteAccount(_testUserId, _testAccountNumber), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public void DeleteAccount_WhenDeletionFails_ReturnFalse()
        {
            // Arrange
            _mockRepository.Setup(db => db.DeleteAccount(_testUserId, _testAccountNumber)).Returns(false);

            // Act
            var result = _accountService.DeleteAccount(_testUserId, _testAccountNumber);

            // Assert
            _mockRepository.Verify(db => db.DeleteAccount(_testUserId, _testAccountNumber), Times.Once);
            Assert.False(result);
        }

        [Fact]
        public void Deposit_WhenDepositAmountIsGreaterThan100000_Limit_ThrowsArgumentException()
        {
            // Arrange
            var deposit = 10000.01m;


            // Act
            var exception = Assert.Throws<ArgumentException>(() => _accountService.Deposit(_testUserId, _testAccountNumber, deposit));

            // Assert
            Assert.Contains("Cannot deposit more than $10,000 in a single transaction.", exception.Message);
        }

        [Fact]
        public void Deposit_DepositSuccessul()
        {
            // Arrange
            var deposit = 1000m;
            var account = new Account { UserId = Guid.NewGuid(), Balance = 5000 };
            _mockRepository.Setup(db => db.UpdateBalance(_testUserId, _testAccountNumber, deposit)).Returns(account);

            // Act
            var result = _accountService.Deposit(_testUserId, _testAccountNumber, deposit);

            // Assert
            _mockRepository.Verify(db => db.UpdateBalance(_testUserId, _testAccountNumber, deposit), Times.Once);
            Assert.Equal(account, result);
        }

        // TODO Withdraw and GetBalance unit tests
    }
}