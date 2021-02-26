using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SettingX.Core.Models;
using SettingX.Repositories.Common.Client;
using Xunit;

namespace SettingX.Repositories.Tests.Rest.RepositoryTests
{
    public class KeyValuesHistoryTest : IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public KeyValuesHistoryTest(ClientFixture fixture)
        {
            _client = fixture.Client;
        }
        
        [Fact]
        public async Task Write_Read()
        {
            var keyValueId = Guid.NewGuid().ToString();
            var newValue = Guid.NewGuid().ToString();
            var keyValues = Guid.NewGuid().ToString();
            var userName = Guid.NewGuid().ToString();
            var userIp = Guid.NewGuid().ToString();

            await _client.KeyValuesHistoryApi.SaveKeyValueHistoryAsync(keyValueId, newValue, keyValues, userName,
                userIp);

            var historicEntity = await _client.KeyValuesHistoryApi.GetHistoryByKeyValueAsync(keyValueId);

            Assert.Single(historicEntity);
            Assert.Equal(keyValueId, historicEntity.Single().KeyValueId);
            Assert.Equal(newValue, historicEntity.Single().NewValue);
            Assert.Equal(userIp, historicEntity.Single().UserIpAddress);
            Assert.Equal(userName, historicEntity.Single().UserName);
            Assert.NotNull(historicEntity.Single().KeyValuesSnapshot);
        }
        
        [Fact]
        public async Task Write_Read2()
        {
            var keyValueId = Guid.NewGuid().ToString();
            var keyValues = Guid.NewGuid().ToString();
            var userName = Guid.NewGuid().ToString();
            var userIp = Guid.NewGuid().ToString();
            var newOverride = Guid.NewGuid().ToString();

            await _client.KeyValuesHistoryApi.SaveKeyValueOverrideHistoryAsync(keyValueId, newOverride, keyValues, userName,
                userIp);

            var historicEntities = await _client.KeyValuesHistoryApi.GetHistoryByKeyValueAsync(keyValueId);

            Assert.Single(historicEntities);
            Assert.Equal(keyValueId, historicEntities.Single().KeyValueId);
            Assert.Equal(newOverride, historicEntities.Single().NewOverride);
            Assert.Equal(userIp, historicEntities.Single().UserIpAddress);
            Assert.Equal(userName, historicEntities.Single().UserName);
            Assert.NotNull(historicEntities.Single().KeyValuesSnapshot);
        }
        
        [Fact]
        public async Task Write_Read3()
        {
            var keyValueId = Guid.NewGuid().ToString();
            var userName = Guid.NewGuid().ToString();
            var userIp = Guid.NewGuid().ToString();

            await _client.KeyValuesHistoryApi.SaveKeyValuesHistoryAsync(
                new List<KeyValue>
                {
                    new KeyValue
                    {
                        Value = Guid.NewGuid().ToString(),
                        KeyValueId = keyValueId,
                        Override = new[]
                        {
                            new OverrideValue {NetworkId = Guid.NewGuid().ToString(), Value = Guid.NewGuid().ToString()}
                        },
                        RepositoryNames = new[] {Guid.NewGuid().ToString()},
                        IsDuplicated = false,
                        EmptyValueType = Guid.NewGuid().ToString(),
                        RepositoryId = Guid.NewGuid().ToString(),
                        Tag = Guid.NewGuid().ToString(),
                        Types = new[] {Guid.NewGuid().ToString()},
                        UseNotTaggedValue = false
                    },
                    new KeyValue
                    {
                        Value = Guid.NewGuid().ToString(),
                        KeyValueId = keyValueId,
                        Override = new[]
                        {
                            new OverrideValue {NetworkId = Guid.NewGuid().ToString(), Value = Guid.NewGuid().ToString()}
                        },
                        RepositoryNames = new[] {Guid.NewGuid().ToString()},
                        IsDuplicated = false,
                        EmptyValueType = Guid.NewGuid().ToString(),
                        RepositoryId = Guid.NewGuid().ToString(),
                        Tag = Guid.NewGuid().ToString(),
                        Types = new[] {Guid.NewGuid().ToString()},
                        UseNotTaggedValue = false
                    }
                }, userName, userIp);

            var historicEntities = await _client.KeyValuesHistoryApi.GetHistoryByKeyValueAsync(keyValueId);

            Assert.Single(historicEntities);
            Assert.Equal(keyValueId, historicEntities.Single().KeyValueId);
            Assert.Equal(userIp, historicEntities.Single().UserIpAddress);
            Assert.Equal(userName, historicEntities.Single().UserName);
            Assert.NotNull(historicEntities.Single().KeyValuesSnapshot);
        }
    }
}