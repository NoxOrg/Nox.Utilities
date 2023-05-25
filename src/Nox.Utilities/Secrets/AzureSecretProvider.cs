using System.Runtime.CompilerServices;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Nox.Utilities.Configuration;
using Nox.Utilities.Credentials;

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
        var credential = await CredentialHelper.GetCredentialFromCacheOrBrowser();

        var secrets = new List<KeyValuePair<string, string>>();

        var secretClient = new SecretClient(_vaultUri, credential.Credential);
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

    
}