using System;
using System.Threading.Tasks;
using SettingX.Core.Models;
using SettingX.Repositories.Common.Client;
using Xunit;
using Xunit.Priority;

namespace SettingX.Repositories.Tests.Rest.RepositoryTests
{
    [DefaultPriority(0)]
    public class RepositoriesTest : IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public RepositoriesTest(ClientFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Write_Read()
        {
            var repository = new Repository
            {
                RepositoryId = Guid.NewGuid().ToString(),
                Branch = Guid.NewGuid().ToString(),
                ConnectionUrl = Guid.NewGuid().ToString(),
                FileName = Guid.NewGuid().ToString(),
                GitUrl = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Tag = Guid.NewGuid().ToString(),
                LastModified = Guid.NewGuid().ToString(),
                OriginalName = Guid.NewGuid().ToString(),
                UserName = Guid.NewGuid().ToString(),
                UseManualSettings = true
            };

            await _client.RepositoriesApi.SaveRepositoryAsync(repository);

            var repositoryFromDb = await _client.RepositoriesApi.GetAsync(repository.RepositoryId);
            
            Assert.True(Equal(repository, repositoryFromDb));
        }
        
        [Fact]
        public async Task Write_Remove_Read()
        {
            var repository = new Repository
            {
                RepositoryId = Guid.NewGuid().ToString(),
                Branch = Guid.NewGuid().ToString(),
                ConnectionUrl = Guid.NewGuid().ToString(),
                FileName = Guid.NewGuid().ToString(),
                GitUrl = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                Tag = Guid.NewGuid().ToString(),
                LastModified = Guid.NewGuid().ToString(),
                OriginalName = Guid.NewGuid().ToString(),
                UserName = Guid.NewGuid().ToString(),
                UseManualSettings = true
            };

            await _client.RepositoriesApi.SaveRepositoryAsync(repository);
            
            await _client.RepositoriesApi.RemoveRepositoryAsync(repository.RepositoryId);

            var repositoryFromDb = await _client.RepositoriesApi.GetAsync(repository.RepositoryId);
            
            Assert.Null(repositoryFromDb);
        }

        [FactConditional, Priority(1)]
        public async Task Read()
        {
            var allRepositories = await _client.RepositoriesApi.GetAllAsync();
            
            Assert.Single(allRepositories);
        }

        private bool Equal(Repository repository1, Repository repository2)
        {
            return repository1.RepositoryId == repository2.RepositoryId &&
                   repository1.UserName == repository2.UserName &&
                   repository1.Branch == repository2.Branch &&
                   repository1.Name == repository2.Name &&
                   repository1.Tag == repository2.Tag &&
                   repository1.ConnectionUrl == repository2.ConnectionUrl &&
                   repository1.FileName == repository2.FileName &&
                   repository1.GitUrl == repository2.GitUrl;
        }
    }
}