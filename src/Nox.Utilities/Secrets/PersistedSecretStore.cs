using Microsoft.AspNetCore.DataProtection;
using Nox.Utilities.Identifier;

namespace Nox.Utilities.Secrets;

public class PersistedSecretStore: IPersistedSecretStore
{
    private readonly IDataProtector _protector;
    private const string ProtectorPurpose = "nox-secrets";

    public PersistedSecretStore(
        IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector(ProtectorPurpose);
    }

    public Task SaveAsync(string key, string secret)
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nox");
        Directory.CreateDirectory(path);
        var keyNuid = new Nuid(key).ToHex();
        path += $"/.{keyNuid}";
        return File.WriteAllTextAsync(path, _protector.Protect(secret));
    }

    public async Task<string?> LoadAsync(string key, TimeSpan? ttl = null)
    {
        ttl ??= new TimeSpan(0, 10, 0);
        var keyNuid = new Nuid(key).ToHex();
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nox", $".{keyNuid}");
        if (!File.Exists(path)) return null;
        //Check if the secret has expired
        var fileInfo = new FileInfo(path);
        if (fileInfo.CreationTime.Add(ttl.Value) < DateTime.Now)
        {
            File.Delete(path);
            return null;
        }
        var content = await File.ReadAllTextAsync(path);
        return _protector.Unprotect(content);
    }
}