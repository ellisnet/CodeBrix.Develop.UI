using System.Collections.Generic;
using CodeBrix.Develop.UI.Generator.Generator;

namespace CodeBrix.Develop.UI.Generator; //was previously: Generator;

public static class Bitfields
{
    public static void Generate(IEnumerable<GirModel.Bitfield> bitfields, string path)
    {
        var publisher = new Publisher(path);
        var generator = new Generator.Public.Bitfield(publisher);

        foreach (var bitfield in bitfields)
            generator.Generate(bitfield);
    }
}
