namespace CodeBrix.Develop.UI.GirModel; //was previously: GirModel;

public interface ComplexType : Type
{
    Namespace Namespace { get; }
    string Name { get; }
}
