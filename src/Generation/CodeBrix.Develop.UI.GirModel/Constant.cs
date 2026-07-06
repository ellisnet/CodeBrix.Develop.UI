namespace CodeBrix.Develop.UI.GirModel; //was previously: GirModel;

public interface Constant
{
    Namespace Namespace { get; }
    string Name { get; }
    string Value { get; }
    Type Type { get; }
    bool Introspectable { get; }
}
