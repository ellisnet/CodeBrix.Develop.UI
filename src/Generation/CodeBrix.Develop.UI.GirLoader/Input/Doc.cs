using System.Xml.Serialization;

namespace CodeBrix.Develop.UI.GirLoader.Input; //was previously: GirLoader.Input;

public class Doc
{
    [XmlText]
    public string? Text { get; set; }
}
