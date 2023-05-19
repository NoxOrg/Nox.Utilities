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
            Assert.That(nuid._id, Is.EqualTo(-1308813586));
            Assert.That(nuid.ToHex(), Is.EqualTo("B1FD16EE"));
            Assert.That(nuid.ToBase36(), Is.EqualTo("6YMVDD1"));
            Assert.That(nuid.ToGuid(), Is.EqualTo(new Guid("00000000-0000-0000-0000-0000b1fd16ee")));
        });
    }
}