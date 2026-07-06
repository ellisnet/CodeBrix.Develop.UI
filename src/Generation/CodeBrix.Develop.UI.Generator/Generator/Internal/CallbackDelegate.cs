using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Generator.Internal; //was previously: Generator.Generator.Internal;

internal class CallbackDelegate : Generator<GirModel.Callback>
{
    private readonly Publisher _publisher;

    public CallbackDelegate(Publisher publisher)
    {
        _publisher = publisher;
    }

    public void Generate(GirModel.Callback callback)
    {
        var source = Renderer.Internal.CallbackDelegate.RenderFile(callback);

        var codeUnit = new CodeUnit(
            Project: Namespace.GetCanonicalName(callback.Namespace),
            Name: callback.Name,
            Source: source,
            IsInternal: true
        );

        _publisher.Publish(codeUnit);
    }
}
