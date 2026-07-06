using System.Xml.Serialization;

namespace CodeBrix.Develop.UI.GirLoader.Input; //was previously: GirLoader.Input;

public class Include
{
    [XmlAttribute("name")]
    public string? Name { get; set; }

    [XmlAttribute("version")]
    public string? Version { get; set; }
}
