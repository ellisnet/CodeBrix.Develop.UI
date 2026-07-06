using System.Xml.Serialization;

namespace CodeBrix.Develop.UI.GirLoader.Input; //was previously: GirLoader.Input;

public class Constant : AnyType
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("value")]
    public string? Value { get; set; }

    [XmlElement("type")]
    public Type? Type { get; set; }

    [XmlElement("array")]
    public ArrayType? Array { get; set; }

    [XmlElement("doc")]
    public Doc? Doc { get; set; }

    [XmlAttribute("introspectable")]
    public bool Introspectable = true;
}
