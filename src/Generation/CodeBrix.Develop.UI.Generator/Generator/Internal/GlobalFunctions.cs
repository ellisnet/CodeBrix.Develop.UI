using System.Collections.Generic;
using System.Linq;
using CodeBrix.Develop.UI.Generator.Model;

namespace CodeBrix.Develop.UI.Generator.Generator.Internal; //was previously: Generator.Generator.Internal;

internal class GlobalFunctions : Generator<IEnumerable<GirModel.Function>>
{
    private readonly Publisher _publisher;

    public GlobalFunctions(Publisher publisher)
    {
        _publisher = publisher;
    }

    public void Generate(IEnumerable<GirModel.Function> functions)
    {
        if (!functions.Any())
            return;

        var source = Renderer.Internal.GlobalFunctions.Render(functions);
        var codeUnit = new CodeUnit(
            Project: Namespace.GetCanonicalName(functions.First().Namespace),
            Name: "Functions",
            Source: source,
            IsInternal: true
        );

        _publisher.Publish(codeUnit);
    }
}
