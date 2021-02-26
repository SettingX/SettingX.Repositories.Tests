using System;
using System.Threading.Tasks;
using SettingX.Core.Models;
using SettingX.Repositories.Common.Client;
using Xunit;

namespace SettingX.Repositories.Tests.Rest.RepositoryTests
{
    public class AccountTokenHistoryTest : IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public AccountTokenHistoryTest(ClientFixture fixture)
        {
            _client = fixture.Client;
        }
        
        [Fact]
        public async Task Write()
        {
            await _client.AccountTokenHistoryApi.SaveTokenHistoryAsync(new Token
            {
                AccessList = Guid.NewGuid().ToString(),
                IpList = Guid.NewGuid().ToString(),
                TokenId = Guid.NewGuid().ToString()
            }, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }
    }
}