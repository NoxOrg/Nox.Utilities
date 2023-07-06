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
        var keys = new string[] { key };
        //Act
        var secrets = provider.GetSecrets(keys);
        //Assert
        Assert.NotNull(secrets);
        Assert.Single(secrets);
        Assert.Equal(expectedResult, secrets.First().Value);
    }

    
}