namespace Nox.Utilities.Secrets;

public interface ISecretProvider
{
    int Pecedence { get; }
    Task<IList<KeyValuePair<string, string>>?> GetSecretsAsync(string[] keys);
    IList<KeyValuePair<string, string>>? GetSecrets(string[] keys);
}