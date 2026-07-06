namespace CodeBrix.Develop.UI.GirModel; //was previously: GirModel;

public interface Alias : Type
{
    Namespace Namespace { get; }
    string Name { get; }
    Type Type { get; }
}
