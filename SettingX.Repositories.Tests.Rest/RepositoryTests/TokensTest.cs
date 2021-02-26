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
    public class TokensTest : IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public TokensTest(ClientFixture fixture)
        {
            _client = fixture.Client;
        }
        
        [Fact]
        public async Task Write_Read_Update_Read()
        {
            var token1 = new Token
            {
                TokenId = Guid.NewGuid().ToString(),
                AccessList = Guid.NewGuid().ToString(),
                IpList = Guid.NewGuid().ToString()
            };
            
            var token2 = new Token
            {
                TokenId = Guid.NewGuid().ToString(),
                AccessList = Guid.NewGuid().ToString(),
                IpList = Guid.NewGuid().ToString()
            };

            await _client.TokensApi.SaveTokenAsync(token1);
            await _client.TokensApi.SaveTokenAsync(token2);

            token1.AccessList = Guid.NewGuid().ToString();
            token1.IpList = Guid.NewGuid().ToString();

            await _client.TokensApi.SaveTokenAsync(token1);

            var token1FromDb = await _client.TokensApi.GetAsync(token1.TokenId);
            var token2FromDb = await _client.TokensApi.GetAsync(token2.TokenId);
            
            Assert.Equal(token1.AccessList, token1FromDb.AccessList);
            Assert.Equal(token1.IpList, token1FromDb.IpList);
            
            Assert.Equal(token2.AccessList, token2FromDb.AccessList);
            Assert.Equal(token2.IpList, token2FromDb.IpList);
        }
        
        [Fact]
        public async Task Write_Remove_Read()
        {
            var token = new Token
            {
                TokenId = Guid.NewGuid().ToString(),
                AccessList = Guid.NewGuid().ToString(),
                IpList = Guid.NewGuid().ToString()
            };

            await _client.TokensApi.SaveTokenAsync(token);

            await _client.ServiceTokensApi.RemoveAsync(token.TokenId);
            
            await _client.ServiceTokensApi.RemoveAsync(token.TokenId);
        
            var tokenFromDb = await _client.ServiceTokensApi.GetAsync(token.TokenId);
            
            Assert.Null(tokenFromDb);
        }
        
        [FactConditional, Priority(1)]
        public async Task Read_All()
        {
            var all = await _client.TokensApi.GetAllAsync();
            
            Assert.Equal(3, all.Count);
        }
    }
}