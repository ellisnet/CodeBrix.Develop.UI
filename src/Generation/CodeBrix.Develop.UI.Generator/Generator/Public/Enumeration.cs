using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Generator.Public; //was previously: Generator.Generator.Public;

internal class Enumeration : Generator<GirModel.Enumeration>
{
    private readonly Publisher _publisher;

    public Enumeration(Publisher publisher)
    {
        _publisher = publisher;
    }

    public void Generate(GirModel.Enumeration obj)
    {
        var source = Renderer.Public.Enumeration.Render(obj);
        var codeUnit = new CodeUnit(
            Project: Namespace.GetCanonicalName(obj.Namespace),
            Name: obj.Name,
            Source: source,
            IsInternal: false
        );

        _publisher.Publish(codeUnit);
    }
}
