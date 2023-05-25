using Nox.Utilities;
using Nox.Utilities.Identifier;

namespace Identifier.Tests;

public class CoreTests
{
    [Test]
    public void Can_create_a_new_Nuid()
    {
        var nuid = new Nuid("Hello World");
        Assert.Multiple(() =>
        {
            Assert.That(nuid._id, Is.EqualTo(421813186));
            Assert.That(nuid.ToHex(), Is.EqualTo("19245BC2"));
            Assert.That(nuid.ToBase36(), Is.EqualTo("Y4X4Z6"));
            Assert.That(nuid.ToGuid(), Is.EqualTo(new Guid("00000000-0000-0000-0000-000019245bc2")));
        });
    }
}