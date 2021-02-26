using System;
using System.Threading.Tasks;
using SettingX.Core.Models;
using SettingX.Repositories.Common.Client;
using Xunit;
using Xunit.Priority;

namespace SettingX.Repositories.Tests.Rest.RepositoryTests
{
    [DefaultPriority(0)]
    public class NetworksTests : IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public NetworksTests(ClientFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Write_Read()
        {
            var network = new Network
            {
                Id = Guid.NewGuid().ToString(),
                Ip = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };

            await _client.NetworksApi.AddAsync(network);

            var networkFromDb = await _client.NetworksApi.GetByIpAsync(network.Ip);
            
            Assert.Equal(network.Id, networkFromDb.Id);
            Assert.Equal(network.Ip, networkFromDb.Ip);
            Assert.Equal(network.Name, networkFromDb.Name);
        }
        
        [Fact]
        public async Task Write_Update_Read()
        {
            var network = new Network
            {
                Id = Guid.NewGuid().ToString(),
                Ip = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };

            await _client.NetworksApi.AddAsync(network);
            
            network.Ip = Guid.NewGuid().ToString();

            await _client.NetworksApi.UpdateAsync(network);

            var networkFromDb = await _client.NetworksApi.GetByIpAsync(network.Ip);
            
            Assert.Equal(network.Id, networkFromDb.Id);
            Assert.Equal(network.Ip, networkFromDb.Ip);
            Assert.Equal(network.Name, networkFromDb.Name);
        }
        
        [Fact]
        public async Task Write_Remove_Read()
        {
            var network = new Network
            {
                Id = Guid.NewGuid().ToString(),
                Ip = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString()
            };

            await _client.NetworksApi.AddAsync(network);

            network.Name = Guid.NewGuid().ToString();
            network.Ip = Guid.NewGuid().ToString();

            await _client.NetworksApi.DeleteAsync(network.Id);

            var networkFromDb = await _client.NetworksApi.GetByIpAsync(network.Ip);
            
            Assert.Null(networkFromDb);
        }
        
        [FactConditional, Priority(1)]
        public async Task Read_All()
        {
            var all = await _client.NetworksApi.GetAllAsync();
            
            Assert.Equal(2, all.Count);
        }
    }
}