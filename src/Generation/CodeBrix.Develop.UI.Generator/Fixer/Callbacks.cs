using System.Collections.Generic;
using CodeBrix.Develop.UI.Generator.Fixer.Callback;

namespace CodeBrix.Develop.UI.Generator.Fixer; //was previously: Generator.Fixer;

public static class Callbacks
{
    private static readonly List<Fixer<GirModel.Callback>> Fixers = [
        new DisableFundamentalReturnTypes()
    ];

    public static void Fixup(IEnumerable<GirModel.Callback> callbacks)
    {
        foreach (var callback in callbacks)
            foreach (var fixer in Fixers)
                fixer.Fixup(callback);
    }
}
