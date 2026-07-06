namespace CodeBrix.Develop.UI.GirLoader.Output; //was previously: GirLoader.Output;

public interface Parameter
{
    string Name { get; }
    Direction Direction { get; }
    bool Nullable { get; }
    bool CallerAllocates { get; }
}
