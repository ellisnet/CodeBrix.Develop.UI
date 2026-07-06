using System.Collections.Generic;
using CodeBrix.Develop.UI.Generator.Generator;

namespace CodeBrix.Develop.UI.Generator; //was previously: Generator;

public static class Aliases
{
    public static void Generate(IEnumerable<GirModel.Alias> aliases, string path)
    {
        var publisher = new Publisher(path);
        var generators = new List<Generator<GirModel.Alias>>
        {
            new Generator.Public.AliasPrimitiveValueType(publisher),
            new Generator.Public.AliasPointer(publisher)
        };

        foreach (var alias in aliases)
            foreach (var generator in generators)
                generator.Generate(alias);
    }
}
