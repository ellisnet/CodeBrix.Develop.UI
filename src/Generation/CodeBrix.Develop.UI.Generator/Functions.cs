using System.Collections.Generic;
using CodeBrix.Develop.UI.Generator.Generator;

namespace CodeBrix.Develop.UI.Generator; //was previously: Generator;

public static class Functions
{
    public static void Generate(IEnumerable<GirModel.Function> functions, string path)
    {
        var publisher = new Publisher(path);
        var generators = new List<Generator<IEnumerable<GirModel.Function>>>()
        {
            new Generator.Internal.GlobalFunctions(publisher),
            new Generator.Public.GlobalFunctions(publisher)
        };

        foreach (var generator in generators)
            generator.Generate(functions);
    }
}
