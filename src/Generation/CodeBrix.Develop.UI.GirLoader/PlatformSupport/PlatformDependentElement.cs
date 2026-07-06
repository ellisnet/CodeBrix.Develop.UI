namespace CodeBrix.Develop.UI.GirLoader.PlatformSupport; //was previously: GirLoader.PlatformSupport;

public class PlatformDependentElement<T>
{
    public T? Linux { get; set; }
    public T? Macos { get; set; }
    public T? Windows { get; set; }
}
