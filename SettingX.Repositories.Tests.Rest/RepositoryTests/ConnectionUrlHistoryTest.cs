using System;
using System.Linq;
using System.Threading.Tasks;
using SettingX.Repositories.Common.Client;
using Xunit;
using Xunit.Priority;

namespace SettingX.Repositories.Tests.Rest.RepositoryTests
{
    [DefaultPriority(0)]
    public class ConnectionUrlHistoryTest : IClassFixture<ClientFixture>
    {
        private readonly IClient _client;

        public ConnectionUrlHistoryTest(ClientFixture fixture)
        {
            _client = fixture.Client;
        }
        
        [Fact]
        public async Task Write_Read1()
        {
            var repositoryId = Guid.NewGuid().ToString();
            var ip = Guid.NewGuid().ToString();
            var userAgent = Guid.NewGuid().ToString();

            await _client.ConnectionUrlHistoryApi.SaveConnectionUrlHistoryAsync(repositoryId, ip, userAgent);

            var historicEvents = await _client.ConnectionUrlHistoryApi.GetByRepositoryIdAsync(repositoryId);

            Assert.Single(historicEvents);
            Assert.True(!string.IsNullOrWhiteSpace(historicEvents.Single().Id));
            Assert.Equal(ip, historicEvents.Single().Ip);
            Assert.Equal(userAgent, historicEvents.Single().UserAgent);
        }
        
        [FactConditional, Priority(1)]
        public async Task Write_Read2()
        {
            var repositoryId = Guid.NewGuid().ToString();
            var ip = Guid.NewGuid().ToString();
            var userAgent = Guid.NewGuid().ToString();

            await _client.ConnectionUrlHistoryApi.SaveConnectionUrlHistoryAsync(repositoryId, ip, userAgent);
            await _client.ConnectionUrlHistoryApi.SaveConnectionUrlHistoryAsync(repositoryId, ip, userAgent);
            await _client.ConnectionUrlHistoryApi.SaveConnectionUrlHistoryAsync(repositoryId, ip, userAgent);
            await _client.ConnectionUrlHistoryApi.SaveConnectionUrlHistoryAsync(repositoryId, ip, userAgent);

            var paged1 = await _client.ConnectionUrlHistoryApi.GetPageAsync(1, 2);
            var paged2 = await _client.ConnectionUrlHistoryApi.GetPageAsync(2, 2);

            Assert.Equal(2, paged1.Events.Count);
            Assert.True(paged1.Total == 5);
            Assert.Equal(2, paged2.Events.Count);
            Assert.True(paged2.Total == 5);
        }
    }
}