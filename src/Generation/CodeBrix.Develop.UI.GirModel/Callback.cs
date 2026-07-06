namespace CodeBrix.Develop.UI.GirModel; //was previously: GirModel;

public interface Callback : ComplexType, Callable
{
    ComplexType? Parent { get; }
    new string Name { get; }
    bool Introspectable { get; }
}
