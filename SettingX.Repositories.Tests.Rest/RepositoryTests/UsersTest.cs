using System;
using System.Linq;
using System.Threading.Tasks;
using SettingX.Core.Models;
using SettingX.Repositories.Common.Client;
using Xunit;
using Xunit.Priority;

namespace SettingX.Repositories.Tests.Rest.RepositoryTests
{
    [DefaultPriority(0)]
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class UsersTest: IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public UsersTest(ClientFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Create_Initial_Admin()
        {
            var email = Guid.NewGuid().ToString();
            var passwordHash = Guid.NewGuid().ToString();

            await _client.UsersApi.CreateInitialAdminAsync(email, passwordHash);

            var user = await _client.UsersApi.GetUserByUserEmailAsync(email, null);
            
            Assert.True(user.Active);
            Assert.True(user.Admin);
            Assert.Equal(email, user.Email);
            Assert.Equal(passwordHash, user.PasswordHash);
            Assert.False(string.IsNullOrWhiteSpace(user.FirstName));
            Assert.False(string.IsNullOrWhiteSpace(user.LastName));
        }
        
        [Fact]
        public async Task Write_Read()
        {
            var email = Guid.NewGuid().ToString();
            var passwordHash = Guid.NewGuid().ToString();
            var firstName = Guid.NewGuid().ToString();
            var lastName = Guid.NewGuid().ToString();
            var salt = Guid.NewGuid().ToString();
            var roles = new [] {Guid.NewGuid().ToString(), Guid.NewGuid().ToString()};
            var admin = false;
            var active = true;

            var initialUser = new User
            {
                Active = active,
                Admin = admin,
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                Salt = salt,
                Roles = roles
            };

            await _client.UsersApi.CreateUserAsync(initialUser);

            var nonExistingUser = await _client.UsersApi.GetUserByUserEmailAsync(Guid.NewGuid().ToString(), null);
            var wrongHashUser = await _client.UsersApi.GetUserByUserEmailAsync(email, Guid.NewGuid().ToString());
            var correctHashUser = await _client.UsersApi.GetUserByUserEmailAsync(email, passwordHash);
            var userWithoutHash = await _client.UsersApi.GetUserByUserEmailAsync(email, null);
            
            Assert.Null(nonExistingUser);
            Assert.Null(wrongHashUser);
            Assert.NotNull(correctHashUser);
            Assert.NotNull(userWithoutHash);
            Assert.True(Equal(correctHashUser, userWithoutHash));
            Assert.True(Equal(initialUser, correctHashUser));
        }
        
        [Fact]
        public async Task Write_Remove_Read()
        {
            var email = Guid.NewGuid().ToString();
            var passwordHash = Guid.NewGuid().ToString();
            var firstName = Guid.NewGuid().ToString();
            var lastName = Guid.NewGuid().ToString();
            var salt = Guid.NewGuid().ToString();
            var roles = new [] {Guid.NewGuid().ToString(), Guid.NewGuid().ToString()};
            var admin = false;
            var active = true;

            var initialUser = new User
            {
                Active = active,
                Admin = admin,
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                Salt = salt,
                Roles = roles
            };

            await _client.UsersApi.CreateUserAsync(initialUser);

            await _client.UsersApi.RemoveUserAsync(email);
            
            var userWithoutHash = await _client.UsersApi.GetUserByUserEmailAsync(email, null);
            
            Assert.Null(userWithoutHash);
        }
        
        [Fact]
        public async Task Write_Update_Read()
        {
            var email = Guid.NewGuid().ToString();
            var passwordHash = Guid.NewGuid().ToString();
            var firstName = Guid.NewGuid().ToString();
            var lastName = Guid.NewGuid().ToString();
            var salt = Guid.NewGuid().ToString();
            var roles = new [] {Guid.NewGuid().ToString(), Guid.NewGuid().ToString()};
            var admin = false;
            var active = true;

            var initialUser = new User
            {
                Active = active,
                Admin = admin,
                Email = email,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                Salt = salt,
                Roles = roles
            };

            await _client.UsersApi.CreateUserAsync(initialUser);

            initialUser.FirstName = Guid.NewGuid().ToString();
            initialUser.LastName = Guid.NewGuid().ToString();
            initialUser.Roles[0] = Guid.NewGuid().ToString();
            initialUser.LastName = Guid.NewGuid().ToString();
            initialUser.Admin = true;
            initialUser.Active = false;
            initialUser.PasswordHash = Guid.NewGuid().ToString();

            await _client.UsersApi.UpdateUserAsync(initialUser);
            
            var userWithoutHash = await _client.UsersApi.GetUserByUserEmailAsync(email, null);
            
            Assert.True(Equal(initialUser, userWithoutHash));
        }
        
        [FactConditional, Priority(1)]
        public async Task Read_All()
        {
            var allUsers = await _client.UsersApi.GetUsersAsync();
            
            Assert.Equal(3, allUsers.Count);
        }

        private static bool Equal(User user1, User user2)
        {
            return user1.Active == user2.Active &&
                   user1.Admin == user2.Admin &&
                   user1.Email == user2.Email &&
                   user1.Roles.All(x => user2.Roles.Contains(x)) &&
                   user2.Roles.All(x => user1.Roles.Contains(x)) &&
                   user1.Salt == user2.Salt &&
                   user1.FirstName == user2.FirstName &&
                   user1.LastName == user2.LastName &&
                   user1.PasswordHash == user2.PasswordHash;
        }
    }
}