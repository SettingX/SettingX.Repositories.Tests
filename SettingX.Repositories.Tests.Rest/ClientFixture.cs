using System;
using System.Text.Json;
using SettingX.Repositories.Common.Client;

namespace SettingX.Repositories.Tests.Rest
{
    public class ClientFixture : IDisposable
    {
        public IClient Client { get; }

        public ClientFixture()
        {
            Client = new Client(
                Environment.GetEnvironmentVariable(Constants.SERVICE_URL)
                    ?? throw new InvalidOperationException($"Need to set up \"{nameof(Constants.SERVICE_URL)}\" environment variable."),
                new JsonSerializerOptions());
        }
        
        public void Dispose()
        {
        }
    }
}