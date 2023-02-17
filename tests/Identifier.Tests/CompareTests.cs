using Nox.Utilities;
using Nox.Utilities.Identifier;

namespace Identifier.Tests;

public class CompareTests
{
    [Test]
    public void Can_Equals_two_Nuids()
    {
        var a = new Nuid("Hello World");
        var b = new Nuid("Hello World");
        var c = new Nuid("Random");
        Assert.That(a == b, Is.True);
        Assert.That(a == c, Is.False);
    }
    
    [Test]
    public void Can_Not_Equals_two_Nuids()
    {
        var a = new Nuid("Hello World");
        var b = new Nuid("Random");
        Assert.That(a != b, Is.True);
    }
    
    [Test]
    public void Can_Compare_two_Nuids()
    {
        var a = new Nuid("Hello World");
        var b = new Nuid("Hello World");
        var c = new Nuid("Random");
        Assert.That(a.CompareTo(b), Is.EqualTo(0));
        Assert.That(a.CompareTo(c), Is.EqualTo(1));
    }

    [Test]
    public void Must_get_exception_when_comparing_nuid_to_another_type()
    {
        var a = new Nuid("Hello World");
        var b = 1;
        Assert.Throws<ArgumentException>(() => a.CompareTo(b));
    }
    
}