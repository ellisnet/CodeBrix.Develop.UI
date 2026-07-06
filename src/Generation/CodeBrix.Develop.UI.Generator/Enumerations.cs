using System.Collections.Generic;
using CodeBrix.Develop.UI.Generator.Generator;

namespace CodeBrix.Develop.UI.Generator; //was previously: Generator;

public static class Enumerations
{
    public static void Generate(IEnumerable<GirModel.Enumeration> enumerations, string path)
    {
        var publisher = new Publisher(path);
        var generator = new Generator.Public.Enumeration(publisher);

        foreach (var enumeration in enumerations)
            generator.Generate(enumeration);
    }
}
