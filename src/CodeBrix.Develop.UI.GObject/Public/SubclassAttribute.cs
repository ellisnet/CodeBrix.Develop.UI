namespace CodeBrix.Develop.UI.GObject; //was previously: GObject;

[System.AttributeUsage(System.AttributeTargets.Class, Inherited = false)]
public class SubclassAttribute<T>(string? qualifiedName = null) : System.Attribute where T : GObject.Object
{
    public string? QualifiedName { get; } = qualifiedName;
}
