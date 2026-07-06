namespace CodeBrix.Develop.UI.GirModel; //was previously: GirModel;

public interface Constructor : Callable
{
    ComplexType Parent { get; }
    string CIdentifier { get; }
    string? Version { get; }
}
