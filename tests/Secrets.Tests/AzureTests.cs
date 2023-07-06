using Microsoft.Extensions.DependencyInjection;
using Nox.Utilities.Secrets;
using Xunit;

namespace Secrets.Tests;

public class AzureTests
{
    [Fact]
    public async Task Can_Retrieve_a_secret()
    {
        var services = new ServiceCollection();
        services.AddAzureSecretProvider("https://nox-EDA1DB500EBCEB02.vault.azure.net/");
        var provider = services.BuildServiceProvider();
        var secretsProvider = provider.GetRequiredService<ISecretProvider>();
        var keys = new string[] { "test-secret" };
        var secrets = await secretsProvider.GetSecretsAsync(keys);
        Assert.NotNull(secrets);
        Assert.Equal(1, secrets.Count);
        Assert.Equal("This is an Organization Secret", secrets[0].Value);
    }
}