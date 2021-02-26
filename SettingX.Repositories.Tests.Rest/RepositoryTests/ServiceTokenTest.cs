using System;
using System.Threading.Tasks;
using SettingX.Core.Models;
using SettingX.Repositories.Common.Client;
using Xunit;
using Xunit.Priority;

namespace SettingX.Repositories.Tests.Rest.RepositoryTests
{
    [DefaultPriority(0)]
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class ServiceTokenTest : IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public ServiceTokenTest(ClientFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Write_Read_Update_Read()
        {
            var token1 = new ServiceToken
            {
                SecurityKeyOne = Guid.NewGuid().ToString(),
                SecurityKeyTwo = Guid.NewGuid().ToString(),
                Token = Guid.NewGuid().ToString()
            };
            
            var token2 = new ServiceToken
            {
                SecurityKeyOne = Guid.NewGuid().ToString(),
                SecurityKeyTwo = Guid.NewGuid().ToString(),
                Token = Guid.NewGuid().ToString()
            };

            await _client.ServiceTokensApi.SaveOrUpdateAsync(token1);
            await _client.ServiceTokensApi.SaveOrUpdateAsync(token2);

            token1.SecurityKeyOne = Guid.NewGuid().ToString();
            token1.SecurityKeyTwo = Guid.NewGuid().ToString();

            await _client.ServiceTokensApi.SaveOrUpdateAsync(token1);

            var token1FromDb = await _client.ServiceTokensApi.GetAsync(token1.Token);
            var token2FromDb = await _client.ServiceTokensApi.GetAsync(token2.Token);
            
            Assert.Equal(token1.SecurityKeyOne, token1FromDb.SecurityKeyOne);
            Assert.Equal(token1.SecurityKeyTwo, token1FromDb.SecurityKeyTwo);
            
            Assert.Equal(token2.SecurityKeyOne, token2FromDb.SecurityKeyOne);
            Assert.Equal(token2.SecurityKeyTwo, token2FromDb.SecurityKeyTwo);
        }
        
        [Fact]
        public async Task Write_Remove_Read()
        {
            var token = new ServiceToken
            {
                SecurityKeyOne = Guid.NewGuid().ToString(),
                SecurityKeyTwo = Guid.NewGuid().ToString(),
                Token = Guid.NewGuid().ToString()
            };

            await _client.ServiceTokensApi.SaveOrUpdateAsync(token);

            await _client.ServiceTokensApi.RemoveAsync(token.Token);
            
            await _client.ServiceTokensApi.RemoveAsync(token.Token);
        
            var token1FromDb = await _client.ServiceTokensApi.GetAsync(token.Token);
            
            Assert.Null(token1FromDb);
        }
        
        [FactConditional, Priority(1)]
        public async Task Read_All()
        {
            var all = await _client.ServiceTokensApi.GetAllAsync();
            
            Assert.Equal(2, all.Count);
        }
    }
}