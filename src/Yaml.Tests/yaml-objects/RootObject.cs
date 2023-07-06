namespace Yaml.Tests.yaml_objects;

public class RootObject
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<SimpleItem>? SimpleList { get; set; }
    public ComplexItem? ComplexItem { get; set; }
    public List<ComplexItem>? ComplexList { get; set; }
    public string Closure { get; set; } = string.Empty;
}