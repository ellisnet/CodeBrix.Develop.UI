using System.Collections.Generic;
using CodeBrix.Develop.UI.Generator.Fixer.Bitfield;

namespace CodeBrix.Develop.UI.Generator.Fixer; //was previously: Generator.Fixer;

public static class Bitfields
{
    private static readonly List<Fixer<GirModel.Bitfield>> Fixers = new()
    {
        new DisableDuplicateMembersFixer()
    };

    public static void Fixup(IEnumerable<GirModel.Bitfield> bitfields)
    {
        foreach (var bitfield in bitfields)
            foreach (var fixer in Fixers)
                fixer.Fixup(bitfield);
    }
}
