using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SettingX.Core.Models;
using SettingX.Repositories.Common.Client;
using Xunit;
using Xunit.Priority;

namespace SettingX.Repositories.Tests.Rest.RepositoryTests
{
    [DefaultPriority(0)]
    public class SecretKeyValueTest : IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public SecretKeyValueTest(ClientFixture fixture)
        {
            _client = fixture.Client;
        }
        
        [Fact]
        public async Task Write_Read_Update_Read()
        {
            var keyValue = new KeyValue
            {
                Value = Guid.NewGuid().ToString(),
                KeyValueId = Guid.NewGuid().ToString(),
                Override = new [] { new OverrideValue {NetworkId = Guid.NewGuid().ToString(), Value = Guid.NewGuid().ToString()} },
                RepositoryNames = new[] {Guid.NewGuid().ToString()},
                IsDuplicated = false,
                EmptyValueType = Guid.NewGuid().ToString(),
                RepositoryId = Guid.NewGuid().ToString(),
                Tag = Guid.NewGuid().ToString(),
                Types = new[] {Guid.NewGuid().ToString()},
                UseNotTaggedValue = false
            };

            await _client.SecretKeyValuesApi.UpdateKeyValueAsync(new List<KeyValue> {keyValue});

            var keyValueFromDb1 = await _client.SecretKeyValuesApi.GetKeyValueAsync(keyValue.KeyValueId);
            var keyValueFromDb2 = await _client.SecretKeyValuesApi.GetKeyValuesAsync(new List<string> {keyValue.KeyValueId});
            
            Assert.True(Equal(keyValue, keyValueFromDb1));
            Assert.Single(keyValueFromDb2);
            Assert.True(Equal(keyValue, keyValueFromDb2[keyValue.KeyValueId]));

            keyValue.Value = Guid.NewGuid().ToString();
            keyValue.RepositoryNames[0] = Guid.NewGuid().ToString();
            keyValue.Override[0].Value = Guid.NewGuid().ToString();
            keyValue.Override[0].NetworkId = Guid.NewGuid().ToString();
            keyValue.Tag = Guid.NewGuid().ToString();
            keyValue.IsDuplicated = true;
            keyValue.EmptyValueType = Guid.NewGuid().ToString();
            keyValue.UseNotTaggedValue = true;
            keyValue.Types[0] = Guid.NewGuid().ToString();
            keyValue.RepositoryId = Guid.NewGuid().ToString();
            
            await _client.SecretKeyValuesApi.ReplaceKeyValueAsync(new List<KeyValue> {keyValue});
            
            keyValueFromDb1 = await _client.SecretKeyValuesApi.GetKeyValueAsync(keyValue.KeyValueId);
            keyValueFromDb2 = await _client.SecretKeyValuesApi.GetKeyValuesAsync(new List<string> {keyValue.KeyValueId});
            
            Assert.True(Equal(keyValue, keyValueFromDb1));
            Assert.Single(keyValueFromDb2);
            Assert.True(Equal(keyValue, keyValueFromDb2[keyValue.KeyValueId]));
        }
        
        [Fact]
        public async Task Write_Find()
        {
            var keyValue = new KeyValue
            {
                Value = Guid.NewGuid().ToString(),
                KeyValueId = Guid.NewGuid().ToString(),
                Override = new [] { new OverrideValue {NetworkId = Guid.NewGuid().ToString(), Value = Guid.NewGuid().ToString()} },
                RepositoryNames = new[] {Guid.NewGuid().ToString()},
                IsDuplicated = false,
                EmptyValueType = Guid.NewGuid().ToString(),
                RepositoryId = Guid.NewGuid().ToString(),
                Tag = Guid.NewGuid().ToString(),
                Types = new[] {Guid.NewGuid().ToString()},
                UseNotTaggedValue = false
            };
            
            await _client.SecretKeyValuesApi.UpdateKeyValueAsync(new List<KeyValue> {keyValue});

            var keyValueFromDb = await _client.SecretKeyValuesApi.GetAsync(null, keyValue.Types[0]);
            
            Assert.True(Equal(keyValue, keyValueFromDb.Single()));
            Assert.Single(keyValueFromDb);
            
            keyValueFromDb = await _client.SecretKeyValuesApi.GetKeyValuesAsync(keyValue.RepositoryNames[0], null, null, null);
            
            Assert.True(Equal(keyValue, keyValueFromDb.Single()));
            Assert.Single(keyValueFromDb);
            
            keyValueFromDb = await _client.SecretKeyValuesApi.GetKeyValuesAsync(null, null, null, keyValue.RepositoryId);
            
            Assert.True(Equal(keyValue, keyValueFromDb.Single()));
            Assert.Single(keyValueFromDb);
            
            keyValueFromDb = await _client.SecretKeyValuesApi.GetKeyValuesAsync(null, null, keyValue.Override[0].Value.Substring(15), null);
            
            Assert.Contains(keyValueFromDb, x => Equal(keyValue, x));
            
            keyValueFromDb = await _client.SecretKeyValuesApi.GetKeyValuesAsync(null, keyValue.RepositoryNames[0], keyValue.Override[0].Value.Substring(15), null);
            
            Assert.True(Equal(keyValue, keyValueFromDb.Single()));
            Assert.Single(keyValueFromDb);
            
            keyValueFromDb = await _client.SecretKeyValuesApi.GetKeyValuesAsync(null, keyValue.RepositoryNames[0], null, null);
            
            Assert.True(Equal(keyValue, keyValueFromDb.Single()));
            Assert.Single(keyValueFromDb);
            
            var emptySearch = await _client.SecretKeyValuesApi.GetKeyValuesAsync(Guid.NewGuid().ToString(), null, null, keyValue.RepositoryId);
            
            Assert.Empty(emptySearch);
            
            emptySearch = await _client.SecretKeyValuesApi.GetKeyValuesAsync(null, null, Guid.NewGuid().ToString(), keyValue.RepositoryId);
            
            Assert.Empty(emptySearch);
            
            emptySearch = await _client.SecretKeyValuesApi.GetKeyValuesAsync(null, Guid.NewGuid().ToString(), null, keyValue.RepositoryId);
            
            Assert.Empty(emptySearch);
        }

        [Fact]
        public async Task Write_Remove_Read_Remove_Read()
        {
            var keyValue = new KeyValue
            {
                Value = Guid.NewGuid().ToString(),
                KeyValueId = Guid.NewGuid().ToString(),
                Override = new [] { new OverrideValue {NetworkId = Guid.NewGuid().ToString(), Value = Guid.NewGuid().ToString()} },
                RepositoryNames = new[] {Guid.NewGuid().ToString()},
                IsDuplicated = false,
                EmptyValueType = Guid.NewGuid().ToString(),
                RepositoryId = Guid.NewGuid().ToString(),
                Tag = Guid.NewGuid().ToString(),
                Types = new[] {Guid.NewGuid().ToString()},
                UseNotTaggedValue = false
            };
            
            await _client.SecretKeyValuesApi.UpdateKeyValueAsync(new List<KeyValue> {keyValue});

            await _client.SecretKeyValuesApi.RemoveNetworkOverridesAsync(keyValue.Override[0].NetworkId);
            
            var keyValueFromDb = await _client.SecretKeyValuesApi.GetAsync(null, keyValue.Types[0]);
            
            Assert.True(keyValueFromDb[0].Override.All(x => x.NetworkId != keyValue.Override[0].NetworkId));

            await _client.SecretKeyValuesApi.DeleteKeyValueWithHistoryAsync(keyValue.KeyValueId, Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            
            keyValueFromDb = await _client.SecretKeyValuesApi.GetAsync(null, keyValue.Types[0]);

            Assert.Empty(keyValueFromDb);
        }
        
        [FactConditional, Priority(1)]
        public async Task Read_All()
        {
            var all = await _client.SecretKeyValuesApi.GetAsync();
            
            Assert.Equal(2, all.Count);
        }

        private static bool Equal(KeyValue keyValue1, KeyValue keyValue2)
        {
            return keyValue1.Override.All(x => keyValue2.Override.Any(y => x.Value == y.Value && x.NetworkId == y.NetworkId)) &&
                   keyValue1.Override.Length == keyValue2.Override.Length &&
                   keyValue1.Tag == keyValue2.Tag &&
                   keyValue1.Types.All(x => keyValue2.Types.Contains(x)) &&
                   keyValue1.Types.Length == keyValue2.Types.Length &&
                   keyValue1.Value == keyValue2.Value &&
                   keyValue1.IsDuplicated == keyValue2.IsDuplicated &&
                   keyValue1.RepositoryId == keyValue2.RepositoryId &&
                   keyValue1.RepositoryNames.All(x => keyValue2.RepositoryNames.Contains(x)) &&
                   keyValue1.RepositoryNames.Length == keyValue2.RepositoryNames.Length &&
                   keyValue1.KeyValueId == keyValue2.KeyValueId &&
                   keyValue1.UseNotTaggedValue == keyValue2.UseNotTaggedValue;
        }
    }
}