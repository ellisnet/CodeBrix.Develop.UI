using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Generator.Internal; //was previously: Generator.Generator.Internal;

internal class OpaqueUntypedRecord : Generator<GirModel.Record>
{
    private readonly Publisher _publisher;

    public OpaqueUntypedRecord(Publisher publisher)
    {
        _publisher = publisher;
    }

    public void Generate(GirModel.Record obj)
    {
        if (!Record.IsOpaqueUntyped(obj))
            return;

        var source = Renderer.Internal.OpaqueUntypedRecord.Render(obj);
        var codeUnit = new CodeUnit(
            Project: Namespace.GetCanonicalName(obj.Namespace),
            Name: obj.Name,
            Source: source,
            IsInternal: true
        );

        _publisher.Publish(codeUnit);
    }
}
