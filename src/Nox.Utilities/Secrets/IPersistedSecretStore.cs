namespace Nox.Utilities.Secrets;

public interface IPersistedSecretStore
{
    Task SaveAsync(string key, string secret);
    Task<string?> LoadAsync(string key, TimeSpan? validFor = null);
}