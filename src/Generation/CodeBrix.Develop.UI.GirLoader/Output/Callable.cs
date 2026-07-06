namespace CodeBrix.Develop.UI.GirLoader.Output; //was previously: GirLoader.Output;

public interface Callable : GirModel.Callable
{
    new string Name { get; }
    ShadowsReference? ShadowsReference { get; }
    ShadowedByReference? ShadowedByReference { get; }
}
