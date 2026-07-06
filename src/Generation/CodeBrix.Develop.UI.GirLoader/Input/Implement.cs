using System.Xml.Serialization;

namespace CodeBrix.Develop.UI.GirLoader.Input; //was previously: GirLoader.Input;

public class Implement
{
    [XmlAttribute("name")]
    public string? Name { get; set; }
}
