using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Generator.Internal; //was previously: Generator.Generator.Internal;

internal class TypedRecordHandle(Publisher publisher) : Generator<GirModel.Record>
{
    public void Generate(GirModel.Record obj)
    {
        if (!Record.IsTyped(obj))
            return;

        if (!Type.IsEnabled(obj))
            return;

        var source = Renderer.Internal.TypedRecordHandle.Render(obj);
        var codeUnit = new CodeUnit(
            Project: Namespace.GetCanonicalName(obj.Namespace),
            Name: Model.TypedRecord.GetInternalHandle(obj),
            Source: source,
            IsInternal: true
        );

        publisher.Publish(codeUnit);
    }
}
