using Microsoft.Extensions.DependencyInjection;
using Nox.Utilities.Secrets;

namespace Secrets.Tests;

public class AzureTests
{
    [Test]
    public async Task Can_Retrieve_a_secret()
    {
        var services = new ServiceCollection();
        services.AddAzureSecretProvider("https://nox-EDA1DB500EBCEB02.vault.azure.net/");
        var provider = services.BuildServiceProvider();
        var secretsProvider = provider.GetRequiredService<ISecretProvider>();
        var keys = new string[] { "test-secret" };
        var secrets = await secretsProvider.GetSecretsAsync(keys);
        Assert.That(secrets, Is.Not.Null);
        Assert.That(secrets.Count, Is.EqualTo(1));
        Assert.That(secrets[0].Value == "This is an Organization Secret");
    }
}