using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Generator.Internal; //was previously: Generator.Generator.Internal;

internal class FundamentalClassStruct : Generator<GirModel.Class>
{
    private readonly Publisher _publisher;

    public FundamentalClassStruct(Publisher publisher)
    {
        _publisher = publisher;
    }

    public void Generate(GirModel.Class obj)
    {
        if (!obj.Fundamental)
            return;

        var source = Renderer.Internal.FundamentalClassStruct.Render(obj);
        var codeUnit = new CodeUnit(
            Project: Namespace.GetCanonicalName(obj.Namespace),
            Name: $"{obj.Name}.Instance.Struct",
            Source: source,
            IsInternal: true
        );

        _publisher.Publish(codeUnit);
    }
}
