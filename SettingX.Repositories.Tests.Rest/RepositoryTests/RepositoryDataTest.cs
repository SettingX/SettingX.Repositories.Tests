using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SettingX.Repositories.Common.Client;
using Xunit;

namespace SettingX.Repositories.Tests.Rest.RepositoryTests
{
    public class RepositoryDataTest: IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public RepositoryDataTest(ClientFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Write_Read()
        {
            var fileName = $"{Guid.NewGuid().ToString()}.txt";
            
            var j = new J {Value = Guid.NewGuid().ToString()};
            var json = JsonConvert.SerializeObject(j);

            await _client.RepositoryDataApi.UpdateAsync(json, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), fileName);

            var jsonFromDb = await _client.RepositoryDataApi.GetDataAsync(fileName);
            var jFromDb = JsonConvert.DeserializeObject<J>(jsonFromDb);
            
            Assert.Equal(j.Value, jFromDb.Value);
        }
        
        [Fact]
        public async Task Write_Remove_Read()
        {
            var fileName = $"{Guid.NewGuid().ToString()}.txt";
            
            var j = new J {Value = Guid.NewGuid().ToString()};
            var json = JsonConvert.SerializeObject(j);

            await _client.RepositoryDataApi.UpdateAsync(json, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), fileName);

            await _client.RepositoryDataApi.DeleteAsync(fileName);

            var jsonFromDb = await _client.RepositoryDataApi.GetDataAsync(fileName);
            var jFromDb = JsonConvert.DeserializeObject<J>(jsonFromDb);
            
            Assert.Null(jFromDb);
        }

        [Fact]
        public async Task Exists()
        {
            var fileName = $"{Guid.NewGuid().ToString()}.txt";
            
            var j = new J {Value = Guid.NewGuid().ToString()};

            var json = JsonConvert.SerializeObject(j);

            await _client.RepositoryDataApi.UpdateAsync(json, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), fileName);

            var existingExists = await _client.RepositoryDataApi.ExistsAsync(fileName);
            var notExistingExists = await _client.RepositoryDataApi.ExistsAsync(Guid.NewGuid().ToString());
            var allExisting = await _client.RepositoryDataApi.GetExistingFileNamesAsync();
            
            Assert.True(existingExists);
            Assert.False(notExistingExists);
            Assert.Contains(allExisting, x => x==fileName);
        }

        private class J
        {
            public string Value { set; get; }
        }
    }
}