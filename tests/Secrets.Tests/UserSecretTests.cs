using System.Reflection;
using Nox.Utilities.Secrets;

namespace Secrets.Tests;

public class UserSecretTests
{
    [Theory]
    [InlineData("SimpleSecret", "This is a simple secret")]
    [InlineData("NestedSecretSection.NestedSecret", "This is a nested secret")]
    [InlineData("NestedSecretSection.DeeperSecretSection.DeeperSecret", "This is a deeper secret")]
    public void Can_retrieve_a_secret(string key, string expectedResult)
    {
        //Arrange
        var provider = new UserSecretProvider(Assembly.GetExecutingAssembly()!);
        var keys = new[] { key };
        //Act
        var secrets = provider.GetSecrets(keys);
        //Assert
        Assert.NotNull(secrets);
        Assert.Single(secrets);
        Assert.Equal(expectedResult, secrets.First().Value);
    }
    
    [Fact]
    public async Task Calling_Async_GetSecrets_ThrowsException()
    {
        var provider = new UserSecretProvider(Assembly.GetExecutingAssembly()!);
        var exception = await Assert.ThrowsAsync<Exception>(async () => _ =
            await provider.GetSecretsAsync(new[] { "dummy" })
        );

        Assert.Equal("Synchronous 'GetSecrets' must be used for user secrets.", exception.Message);
    }

    
}