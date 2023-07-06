namespace Nox.Utilities.Secrets;

public interface ISecretProvider
{
    Task<IList<KeyValuePair<string, string>>?> GetSecretsAsync(string[] keys);
    IList<KeyValuePair<string, string>>? GetSecrets(string[] keys);
}