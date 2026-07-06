using System.Linq;
using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Generator.Public; //was previously: Generator.Generator.Public;

internal class InterfaceImplementationSignals(Publisher publisher) : Generator<GirModel.Interface>
{
    public void Generate(GirModel.Interface obj)
    {
        if (!obj.Signals.Any())
            return;

        var source = Renderer.Public.InterfaceImplementationSignals.Render(obj);
        var codeUnit = new CodeUnit(
            Project: Namespace.GetCanonicalName(obj.Namespace),
            Name: $"{Model.Interface.GetImplementationName(obj)}.Signals",
            Source: source,
            IsInternal: false
        );

        publisher.Publish(codeUnit);
    }
}
