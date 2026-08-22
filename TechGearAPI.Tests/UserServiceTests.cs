using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TechGearAPI.Data;
using TechGearAPI.DTOs;
using TechGearAPI.Services;
using Xunit;

namespace TechGearAPI.Tests
{
    public class UserServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private IConfiguration GetTestConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                { "Jwt:Key", "this-is-a-test-secret-key-that-is-long-enough-1234" },
                { "Jwt:Issuer", "TestIssuer" },
                { "Jwt:Audience", "TestAudience" }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public async Task RegisterUser_HashesPassword_NotStoredAsPlainText()
        {
            var db = GetDbContext();
            var service = new UserService(db, GetTestConfiguration());
            var dto = new RegisterUserDto
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "PlainTextPassword123!"
            };

            await service.RegisterUser(dto);

            var storedUser = await db.Users.FirstAsync(u => u.Email == "test@example.com");
            Assert.NotEqual("PlainTextPassword123!", storedUser.Password);
            Assert.True(BCrypt.Net.BCrypt.Verify("PlainTextPassword123!", storedUser.Password));
        }

        [Fact]
        public async Task RegisterUser_DefaultsRoleToCustomer()
        {
            var db = GetDbContext();
            var service = new UserService(db, GetTestConfiguration());
            var dto = new RegisterUserDto { Name = "Test", Email = "role@example.com", Password = "Password123!" };

            var result = await service.RegisterUser(dto);

            Assert.Equal("Customer", result.Role);
        }

        [Fact]
        public async Task RegisterUser_WithDuplicateEmail_ThrowsException()
        {
            var db = GetDbContext();
            var service = new UserService(db, GetTestConfiguration());
            var dto = new RegisterUserDto { Name = "Test", Email = "dupe@example.com", Password = "Password123!" };

            await service.RegisterUser(dto); // first registration succeeds

            await Assert.ThrowsAsync<Exception>(() => service.RegisterUser(dto)); // second should throw
        }

        [Fact]
        public async Task LoginUser_WithValidCredentials_ReturnsToken()
        {
            var db = GetDbContext();
            var service = new UserService(db, GetTestConfiguration());
            var registerDto = new RegisterUserDto { Name = "Test", Email = "login@example.com", Password = "CorrectPassword123!" };
            await service.RegisterUser(registerDto);

            var loginDto = new LoginUserDto { Email = "login@example.com", Password = "CorrectPassword123!" };
            var result = await service.LoginUser(loginDto);

            Assert.NotNull(result.Token);
            Assert.NotEmpty(result.Token);
            Assert.Equal("Customer", result.Role);
        }

        [Fact]
        public async Task LoginUser_WithWrongPassword_ThrowsException()
        {
            var db = GetDbContext();
            var service = new UserService(db, GetTestConfiguration());
            var registerDto = new RegisterUserDto { Name = "Test", Email = "wrongpass@example.com", Password = "CorrectPassword123!" };
            await service.RegisterUser(registerDto);

            var loginDto = new LoginUserDto { Email = "wrongpass@example.com", Password = "WrongPassword!" };

            await Assert.ThrowsAsync<Exception>(() => service.LoginUser(loginDto));
        }

        [Fact]
        public async Task LoginUser_WithNonexistentEmail_ThrowsException()
        {
            var db = GetDbContext();
            var service = new UserService(db, GetTestConfiguration());
            var loginDto = new LoginUserDto { Email = "doesnotexist@example.com", Password = "Whatever123!" };

            await Assert.ThrowsAsync<Exception>(() => service.LoginUser(loginDto));
        }
    }
}