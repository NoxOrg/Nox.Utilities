using System.Runtime.CompilerServices;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Nox.Utilities.Configuration;

namespace Nox.Utilities.Secrets;

public class AzureSecretProvider: ISecretProvider
{
    private readonly Uri _vaultUri;

    public AzureSecretProvider(string vaultUrl)
    {
        if (Uri.IsWellFormedUriString(vaultUrl, UriKind.Absolute))
        {
            _vaultUri = new Uri(vaultUrl);
        }
        else
        {
            throw new Exception("VaultUrl is not a well formed Uri!");
        }
    }
    
    public async Task<IList<KeyValuePair<string, string>>?> GetSecretsAsync(string[] keys)
    {
        var credential = (await GetCredentialFromCacheOrBrowser()).Credential;

        var secrets = new List<KeyValuePair<string, string>>();

        var secretClient = new SecretClient(_vaultUri, credential);
        try
        {
            foreach (var key in keys)
            {
                try
                {
                    var secret = await secretClient.GetSecretAsync(key.Replace(":", "--").Replace("_", "-"));
                    secrets.Add(new KeyValuePair<string, string>(key, secret.Value.Value ?? ""));
                }
                catch 
                {
                    //Ignore
                }
            }
        }
        catch (Exception ex)
        {
            string InterpolateError()
            {
                var interpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 2);
                interpolatedStringHandler.AppendLiteral("Error loading secrets from vault at '");
                interpolatedStringHandler.AppendFormatted(_vaultUri);
                interpolatedStringHandler.AppendLiteral("'. (");
                interpolatedStringHandler.AppendFormatted(ex.Message);
                interpolatedStringHandler.AppendLiteral(")");
                return interpolatedStringHandler.ToStringAndClear();
            }

            var errorMessage = InterpolateError();
            throw new Exception(errorMessage);
        }
        return secrets;
    }

    private const string TOKEN_CACHE_NAME = "NoxCliToken";

    public static async 
        Task<(TokenCredential Credential, AuthenticationRecord AuthenticationRecord)> 
        GetCredentialFromCacheOrBrowser()
    {
        var cacheTokenFile = WellKnownPaths.CacheTokenFile;

        InteractiveBrowserCredential credential;
        AuthenticationRecord authRecord;

        if (cacheTokenFile == null || !File.Exists(cacheTokenFile))
        {
            credential = new InteractiveBrowserCredential(
                new InteractiveBrowserCredentialOptions
                {
                    TokenCachePersistenceOptions = new TokenCachePersistenceOptions
                    { Name = TOKEN_CACHE_NAME }
                });

            authRecord = await credential.AuthenticateAsync();

            if (cacheTokenFile != null)
            {
                using var authRecordStream = new FileStream(cacheTokenFile, FileMode.Create, FileAccess.Write);
                await authRecord.SerializeAsync(authRecordStream);
            }
        }
        else
        {
            using var authRecordStream = new FileStream(cacheTokenFile, FileMode.Open, FileAccess.Read);
            authRecord = await AuthenticationRecord.DeserializeAsync(authRecordStream);

            credential = new InteractiveBrowserCredential(
                new InteractiveBrowserCredentialOptions
                {
                    TokenCachePersistenceOptions = new TokenCachePersistenceOptions
                    { Name = TOKEN_CACHE_NAME },
                    AuthenticationRecord = authRecord
                }
            );
        }

        return (credential, authRecord);
    }
}