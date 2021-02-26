using System;
using Xunit;

namespace SettingX.Repositories.Tests.Rest
{
    public sealed class FactConditionalAttribute : FactAttribute
    {
        public FactConditionalAttribute() {
            if(!HasDedicatedDb()) {
                Skip = "Special repositories not set up for tests.";
            }
        }
    
        private static bool HasDedicatedDb() => bool.Parse(Environment.GetEnvironmentVariable(Constants.DEDICATED_DB) ?? string.Empty);
    }
}