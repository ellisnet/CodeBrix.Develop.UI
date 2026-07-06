using System.Xml.Serialization;

namespace CodeBrix.Develop.UI.GirLoader.Input; //was previously: GirLoader.Input;

public class InstanceParameterInfo : AnyType
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("transfer-ownership")]
    public string? TransferOwnership { get; set; }

    [XmlAttribute("direction")]
    public string? Direction { get; set; }

    [XmlAttribute("caller-allocates")]
    public bool CallerAllocates;

    [XmlElement("doc")]
    public Doc? Doc { get; set; }

    [XmlElement("type")]
    public Type? Type { get; set; }

    [XmlElement("array")]
    public ArrayType? Array { get; set; }

    [XmlAttribute("nullable")]
    public bool Nullable;
}
