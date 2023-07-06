using Azure.Core;
using Nox.Utilities.Credentials;

namespace Credentials.Tests;

public class CredentialHelperTests
{
    [Test]
    public async Task Cen_get_credentials_from_cache_or_browser()
    {
        var credentials = await CredentialHelper.GetCredentialFromCacheOrBrowser();
        Assert.Multiple(() =>
        {
            Assert.That(credentials.Credential, Is.Not.Null);
            Assert.That(credentials.AuthenticationRecord, Is.Not.Null);
        });
    }

    [Test]
    public async Task Can_get_azure_devops_pat()
    {
        var token = await CredentialHelper.GetAzureDevOpsAccessToken();
        Assert.That(token, Is.Not.Null);

    }
}