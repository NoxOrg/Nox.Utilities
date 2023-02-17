using Microsoft.Extensions.DependencyInjection;
using Nox.Utilities.Identifier;
using Nox.Utilities.Secrets;

namespace Secrets.Tests;

public class CacheTests
{
    [Test]
    public async Task Can_Save_and_retrieve_a_secret()
    {
        var services = new ServiceCollection();
        services.AddPersistedSecretStore();
        var provider = services.BuildServiceProvider();
        var store = provider.GetRequiredService<IPersistedSecretStore>();
        var key = "my-secret";
        var secret = "This is no secret!";
        await store.SaveAsync(key, secret);
        var nuid = new Nuid(key).ToHex();
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nox", $".{nuid}");
        Assert.That(File.Exists(path), Is.True);
        var loaded = await store.LoadAsync(key, new TimeSpan(0, 0, 1));
        Assert.That(secret, Is.EqualTo(loaded));
    }
    
    [Test]
    public async Task Must_not_retrieve_an_expired_secret()
    {
        var services = new ServiceCollection();
        services.AddPersistedSecretStore();
        var provider = services.BuildServiceProvider();
        var store = provider.GetRequiredService<IPersistedSecretStore>();
        var key = "my-secret";
        var secret = "This is no secret!";
        await store.SaveAsync(key, secret);
        var nuid = new Nuid(key).ToHex();
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nox", $".{nuid}");
        Assert.That(File.Exists(path), Is.True);
        Thread.Sleep(1000);
        var loaded = await store.LoadAsync(key, new TimeSpan(0, 0, 1));
        Assert.That(loaded, Is.Null);
    }
}