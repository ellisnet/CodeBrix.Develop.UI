using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Generator.Internal; //was previously: Generator.Generator.Internal;

internal class FrameworkExtensions : Generator<GirModel.Namespace>
{
    private readonly Publisher _publisher;

    public FrameworkExtensions(Publisher publisher)
    {
        _publisher = publisher;
    }

    public void Generate(GirModel.Namespace ns)
    {
        var source = Renderer.Internal.FrameworkExtensions.Render(ns);
        var codeUnit = new CodeUnit(
            Project: Namespace.GetCanonicalName(ns),
            Name: "Extensions",
            Source: source,
            IsInternal: true
        );

        _publisher.Publish(codeUnit);
    }
}
