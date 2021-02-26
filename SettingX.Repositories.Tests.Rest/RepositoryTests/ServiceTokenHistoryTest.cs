using System;
using System.Threading.Tasks;
using SettingX.Core.Models;
using SettingX.Repositories.Common.Client;
using Xunit;

namespace SettingX.Repositories.Tests.Rest.RepositoryTests
{
    public class ServiceTokenHistoryTest : IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public ServiceTokenHistoryTest(ClientFixture fixture)
        {
            _client = fixture.Client;
        }
        
        [Fact]
        public async Task Write()
        {
            await _client.ServiceTokenHistoryApi.SaveTokenHistoryAsync(
                new ServiceToken
                {
                    SecurityKeyOne = Guid.NewGuid().ToString(),
                    SecurityKeyTwo = Guid.NewGuid().ToString(),
                    Token = Guid.NewGuid().ToString()
                }, 
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString());
        }
    }
}