using System;
using System.Threading.Tasks;
using SettingX.Core.Models;
using SettingX.Repositories.Common.Client;
using Xunit;

namespace SettingX.Repositories.Tests.Rest.RepositoryTests
{
    public class UserActionHistoryTest : IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public UserActionHistoryTest(ClientFixture fixture)
        {
            _client = fixture.Client;
        }
        
        [Fact]
        public async Task Write()
        {
            await _client.UserActionsHistoryApi.SaveUserActionHistoryAsync(
                new UserActionHistory
                {
                    ActionDate = DateTime.UtcNow,
                    ActionName = Guid.NewGuid().ToString(),
                    ControllerName = Guid.NewGuid().ToString(),
                    IpAddress = Guid.NewGuid().ToString(),
                    Params = Guid.NewGuid().ToString(),
                    UserEmail = Guid.NewGuid().ToString()
                });
        }
    }
}