using System.Collections.Generic;
using CodeBrix.Develop.UI.Generator.Generator;

namespace CodeBrix.Develop.UI.Generator; //was previously: Generator;

public static class Unions
{
    public static void Generate(IEnumerable<GirModel.Union> unions, string path)
    {
        var publisher = new Publisher(path);
        var generators = new List<Generator<GirModel.Union>>()
        {
            new Generator.Internal.UnionStruct(publisher),
            new Generator.Internal.UnionMethods(publisher),
        };

        foreach (var union in unions)
            foreach (var generator in generators)
                generator.Generate(union);
    }
}
