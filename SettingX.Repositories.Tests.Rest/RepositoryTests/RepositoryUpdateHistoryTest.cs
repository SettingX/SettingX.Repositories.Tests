using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SettingX.Core.Models;
using SettingX.Repositories.Common.Client;
using Xunit;

namespace SettingX.Repositories.Tests.Rest.RepositoryTests
{
    public class RepositoryUpdateHistoryTest : IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public RepositoryUpdateHistoryTest(ClientFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Write_Read_Write_Read()
        {
            var historicEvent = new RepositoryUpdateHistoricEvent
            {
                Branch = Guid.NewGuid().ToString(),
                CreatedAt = DateTimeOffset.UtcNow,
                InitialCommit = Guid.NewGuid().ToString(),
                RepositoryId = Guid.NewGuid().ToString(),
                IsManual = false,
                User = Guid.NewGuid().ToString()
            };

            await _client.RepositoriesUpdateHistoryApi.SaveRepositoryUpdateHistory(historicEvent);

            var eventFromDb = await _client.RepositoriesUpdateHistoryApi.GetAsync(historicEvent.RepositoryId);
            
            Assert.True(Equal(historicEvent, eventFromDb));
            
            var historicEvent2 = new RepositoryUpdateHistoricEvent
            {
                Branch = Guid.NewGuid().ToString(),
                CreatedAt = DateTimeOffset.UtcNow,
                InitialCommit = historicEvent.InitialCommit,
                RepositoryId = Guid.NewGuid().ToString(),
                IsManual = false,
                User = Guid.NewGuid().ToString()
            };

            await _client.RepositoriesUpdateHistoryApi.SaveRepositoryUpdateHistory(historicEvent2);

            var bothEvents =
                await _client.RepositoriesUpdateHistoryApi.GetByInitialCommitAsync(historicEvent.InitialCommit);
            
            Assert.Equal(2, bothEvents.Count);
        }
        
        [Fact]
        public async Task Write_Remove_Read()
        {
            var historicEvent = new RepositoryUpdateHistoricEvent
            {
                Branch = Guid.NewGuid().ToString(),
                CreatedAt = DateTimeOffset.UtcNow,
                InitialCommit = Guid.NewGuid().ToString(),
                RepositoryId = Guid.NewGuid().ToString(),
                IsManual = false,
                User = Guid.NewGuid().ToString()
            };

            await _client.RepositoriesUpdateHistoryApi.SaveRepositoryUpdateHistory(historicEvent);
            
            await _client.RepositoriesUpdateHistoryApi.RemoveRepositoryUpdateHistoryAsync(new List<string> {historicEvent.RepositoryId});

            var eventFromDb = await _client.RepositoriesUpdateHistoryApi.GetAsync(historicEvent.RepositoryId);
            
            Assert.Null(eventFromDb);
        }

        private bool Equal(RepositoryUpdateHistoricEvent event1, RepositoryUpdateHistoricEvent event2)
        {
            return event1.IsManual == event2.IsManual &&
                   event1.Branch == event2.Branch &&
                   event1.User == event2.User &&
                   event1.CreatedAt == event2.CreatedAt &&
                   event1.InitialCommit == event2.InitialCommit &&
                   event1.RepositoryId == event2.RepositoryId;
        }
    }
}