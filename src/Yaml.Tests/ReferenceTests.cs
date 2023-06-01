using Nox.Utilities.Yaml;
using Yaml.Tests.yaml_objects;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Yaml.Tests;

public class ReferenceTests
{
    [Test]
    public void Can_deserialize_yaml_with_ref_tag()
    {
        var resolver = new YamlResolver("./yaml-files/rootObject.yaml");
        var yaml = resolver.ResolveReferences();
        var builder = new DeserializerBuilder();
        var deserializer = builder
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var parent = deserializer.Deserialize<RootObject>(yaml);
        Assert.NotNull(parent);
        Assert.That(parent.Name, Is.EqualTo("This is the root yaml file"));
        Assert.That(parent.Description, Is.EqualTo("This file contains $ref tags"));
        Assert.That(parent.SimpleList, Is.Not.Null);
        Assert.That(parent.SimpleList!.Count, Is.EqualTo(4));
        Assert.That(parent.ComplexItem, Is.Not.Null);
        Assert.That(parent.ComplexItem!.Name, Is.EqualTo("This is the name of the item"));
        Assert.That(parent.ComplexItem!.Description, Is.EqualTo("This is the description of the item"));
        Assert.That(parent.ComplexItem!.AdditionalInfo, Is.EqualTo("This is the additional information"));
        Assert.That(parent.ComplexList, Is.Not.Null);
        Assert.That(parent.ComplexList!.Count, Is.EqualTo(2));
        Assert.That(parent.ComplexList[0].Name, Is.EqualTo("This is the name of the item"));
        Assert.That(parent.ComplexList[0].Description, Is.EqualTo("This is the description of the item"));
        Assert.That(parent.ComplexList[0].AdditionalInfo, Is.EqualTo("This is the additional information"));
        Assert.That(parent.ComplexList[1].Name, Is.EqualTo("This is the name of the item"));
        Assert.That(parent.ComplexList[1].Description, Is.EqualTo("This is the description of the item"));
        Assert.That(parent.ComplexList[1].AdditionalInfo, Is.EqualTo("This is the additional information"));
        Assert.That(parent.Closure, Is.EqualTo("This is the end of the yaml"));
    }
    
    [Test]
    public void Can_deserialize_yaml_with_commented_ref_tag()
    {
        var resolver = new YamlResolver("./yaml-files/commented.yaml");
        var yaml = resolver.ResolveReferences();
        var builder = new DeserializerBuilder();
        var deserializer = builder
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        var parent = deserializer.Deserialize<YamlParent>(yaml);
        Assert.NotNull(parent);
        Assert.That(parent.AttributeOne.Count, Is.EqualTo(4));
        Assert.That(parent.AttributeTwo.Count, Is.EqualTo(0));
        Assert.That(parent.Name, Is.EqualTo("This is the parent yaml file with a commented $ref"));
        Assert.That(parent.Description, Is.EqualTo("This file contains a $ref tag"));
        Assert.That(parent.Closure, Is.EqualTo("This is the end of the line"));
    }
    
    
}