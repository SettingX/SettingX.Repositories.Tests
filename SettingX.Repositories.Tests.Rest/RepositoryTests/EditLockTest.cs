using System;
using System.Threading.Tasks;
using SettingX.Repositories.Common.Client;
using Xunit;
using Xunit.Priority;

namespace SettingX.Repositories.Tests.Rest.RepositoryTests
{
    public class EditLockTest : IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public EditLockTest(ClientFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact, Priority(0)]
        public async Task Write_Read()
        {
            var userName = Guid.NewGuid().ToString();
            var userIp = Guid.NewGuid().ToString();
            var userEmail = Guid.NewGuid().ToString();

            await _client.LocksApi.SetJsonPageLockAsync(userEmail, userName, userIp);

            var editLock = await _client.LocksApi.GetJsonPageLockAsync();
            
            Assert.Equal(userEmail, editLock.UserEmail);
            Assert.Equal(userIp, editLock.IpAddress);
            Assert.Equal(userName, editLock.UserName);
        }

        [Fact, Priority(1)]
        public async Task Write_Remove_Read()
        {
            var userName = Guid.NewGuid().ToString();
            var userIp = Guid.NewGuid().ToString();
            var userEmail = Guid.NewGuid().ToString();

            await _client.LocksApi.SetJsonPageLockAsync(userEmail, userName, userIp);

            await _client.LocksApi.ResetJsonPageLockAsync();

            var editLock = await _client.LocksApi.GetJsonPageLockAsync();
            
            Assert.Null(editLock.UserEmail);
            Assert.Null(editLock.IpAddress);
            Assert.Null(editLock.UserName);
        }
    }
}