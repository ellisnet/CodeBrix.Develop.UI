using System.Collections.Generic;
using CodeBrix.Develop.UI.Generator.Generator;

namespace CodeBrix.Develop.UI.Generator; //was previously: Generator;

public static class Constants
{
    public static void Generate(IEnumerable<GirModel.Constant> constants, string path)
    {
        var publisher = new Publisher(path);
        var generator = new Generator.Public.Constants(publisher);
        generator.Generate(constants);
    }
}
