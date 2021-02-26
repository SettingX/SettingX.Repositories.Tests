using System;
using System.Threading.Tasks;
using SettingX.Core.Models;
using SettingX.Repositories.Common.Client;
using Xunit;

namespace SettingX.Repositories.Tests.Rest.RepositoryTests
{
    public class UserSignInHistoryTest : IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public UserSignInHistoryTest(ClientFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Write()
        {
            var email = Guid.NewGuid().ToString();
            var passwordHash = Guid.NewGuid().ToString();
            var firstName = Guid.NewGuid().ToString();
            var lastName = Guid.NewGuid().ToString();
            var salt = Guid.NewGuid().ToString();
            var roles = new[] {Guid.NewGuid().ToString(), Guid.NewGuid().ToString()};
            var admin = false;
            var active = true;

            await _client.UserSignInHistoryApi.SaveUserLoginAsync(
                new User
                {
                    Active = active,
                    Admin = admin,
                    Email = email,
                    PasswordHash = passwordHash,
                    FirstName = firstName,
                    LastName = lastName,
                    Salt = salt,
                    Roles = roles
                },
                Guid.NewGuid().ToString());
        }
    }
}