using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Generator.Internal; //was previously: Generator.Generator.Internal;

internal class CallbackNotifiedHandler(Publisher publisher) : Generator<GirModel.Callback>
{
    public void Generate(GirModel.Callback callback)
    {
        if (!Callback.IsEnabled(callback))
            return;

        var source = Renderer.Internal.CallbackNotifiedHandler.RenderFile(callback);
        var codeUnit = new CodeUnit(
            Project: Namespace.GetCanonicalName(callback.Namespace),
            Name: $"{callback.Name}.NotifiedHandler",
            Source: source,
            IsInternal: true
        );

        publisher.Publish(codeUnit);
    }
}
